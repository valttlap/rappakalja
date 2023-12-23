using Microsoft.AspNetCore.SignalR;
using Sanasoppa.Core.DTOs;
using Sanasoppa.Core.Services;
using Sanasoppa.API.Extensions;

namespace Sanasoppa.API.Hubs;

public class GameHub : Hub
{
    private readonly GameService _gameService;
    private readonly PlayerService _playerService;
    private readonly RoundService _roundService;
    private readonly SubmissionService _submissionService;

    public GameHub(GameService gameService, PlayerService playerService, RoundService roundService, SubmissionService submissionService)
    {
        _gameService = gameService;
        _playerService = playerService;
        _roundService = roundService;
        _submissionService = submissionService;
    }

    public async Task<string> JoinGame(string joinCode, string name)
    {
        joinCode = joinCode.ToLower();
        var gameSession = await _gameService.GetGameSessionByJoinCodeAsync(joinCode);
        var player = new PlayerDto()
        {
            Id = Guid.NewGuid().ToString(),
            Name = name,
            ConnectionId = Context.ConnectionId,
            GameSessionId = gameSession.Id,
        };

        await _playerService.CreatePlayerAsync(player);
        await Groups.AddToGroupAsync(Context.ConnectionId, gameSession.Id);
        await Clients.Group(gameSession.Id).SendAsync("PlayerJoined", player);

        return gameSession.Id;
    }

    public async Task StartGame(string gameId)
    {
        var gameGuid = Guid.TryParse(gameId, out var id) ? id : throw new ArgumentException("Invalid game id");
        var gameSession = await _gameService.GetGameSessionByIdAsync(gameGuid);
        var player = await _playerService.GetPlayerByConnectionIdAsync(Context.ConnectionId);
        if (player.Id != gameSession.OwnerId)
        {
            throw new InvalidOperationException("Only the owner of the game can start the game");
        }
        await _gameService.StartGameSessionAsync(gameGuid);
        await Clients.Group(gameId).SendAsync("GameStarted", gameSession);

        var roundDto = new RoundDto()
        {
            Id = Guid.NewGuid().ToString(),
            GameSessionId = gameId,
            RoundNumber = 1,
            LeaderId = player.Id
        };

        await _roundService.CreateAsync(roundDto);
        await Clients.Client(player.ConnectionId).SendAsync("SubmitWord", roundDto);
    }

    public async Task SubmitWord(string gameId, string word)
    {
        var gameGuid = Guid.TryParse(gameId, out var id) ? id : throw new ArgumentException("Invalid game id");
        var player = await _playerService.GetPlayerByConnectionIdAsync(Context.ConnectionId);
        var round = await _roundService.GetCurrentRoundByGameSessionIdAsync(gameGuid) ?? throw new InvalidOperationException("No round found");
        if (player.Id != round.LeaderId)
        {
            throw new InvalidOperationException("Only the leader can submit a word");
        }
        var roundGuid = Guid.TryParse(round.Id, out var roundId) ? roundId : throw new ArgumentException("Invalid round id");
        await _roundService.AddWordToRoundAsync(roundGuid, word);
        await Clients.Group(gameId).SendAsync("WordSubmitted", word);
    }

    public async Task SubmitSubmission(string gameId, string submission)
    {
        var gameGuid = Guid.TryParse(gameId, out var id) ? id : throw new ArgumentException("Invalid game id");
        var player = await _playerService.GetPlayerByConnectionIdAsync(Context.ConnectionId);
        var round = await _roundService.GetCurrentRoundByGameSessionIdAsync(gameGuid) ?? throw new InvalidOperationException("No round found");
        var submissionDto = new SubmissionDto()
        {
            Id = Guid.NewGuid().ToString(),
            PlayerId = player.Id,
            Guess = submission,
            RoundId = round.Id
        };
        await _submissionService.CreateAsync(submissionDto);
        if (await _roundService.AllPlayersSubmittedAsync(Guid.Parse(round.Id)))
        {
            await Clients.Group(gameId).SendAsync("AllPlayersSubmitted");
            var leader = await _playerService.GetPlayerByIdAsync(Guid.Parse(round.LeaderId));
            var submissions = await _submissionService.GetSubmissionsByRoundIdAsync(Guid.Parse(round.Id));
            submissions = submissions.Scramble();
            await Clients.Client(leader.ConnectionId).SendAsync("ReadSubmissions", submissions);
        }
        await Clients.Group(gameId).SendAsync("SubmissionSubmitted", await _roundService.GetSubmissionsDoneAsync(Guid.Parse(round.Id)));
    }

    public async Task RestartRound(string gameId)
    {
        var player = await _playerService.GetPlayerByConnectionIdAsync(Context.ConnectionId);
        var round = await _roundService.GetCurrentRoundByGameSessionIdAsync(Guid.Parse(gameId)) ?? throw new InvalidOperationException("No round found");
        if (player.Id != round.LeaderId)
        {
            throw new InvalidOperationException("Only the leader can restart the round");
        }
        var newRound = await _roundService.ResetRoundAsync(Guid.Parse(round.Id));
        await Clients.Client(player.ConnectionId).SendAsync("SubmitWord", newRound);

    }

    public async Task EndRound(string gameId)
    {
        var player = await _playerService.GetPlayerByConnectionIdAsync(Context.ConnectionId);
        var round = await _roundService.GetCurrentRoundByGameSessionIdAsync(Guid.Parse(gameId)) ?? throw new InvalidOperationException("No round found");
        if (player.Id != round.LeaderId)
        {
            throw new InvalidOperationException("Only the leader can restart the round");
        }
        var newRound = await _roundService.StartNewRoundAsync(gameId);
        await Clients.Group(gameId).SendAsync("RoundEnded", newRound);
        var newLeader = await _playerService.GetPlayerByIdAsync(Guid.Parse(newRound.LeaderId));
        await Clients.Client(newLeader.ConnectionId).SendAsync("NewLeader", newRound);
    }



    public async Task StartVoting(string gameId)
    {
        var gameGuid = Guid.TryParse(gameId, out var id) ? id : throw new ArgumentException("Invalid game id");
        var player = await _playerService.GetPlayerByConnectionIdAsync(Context.ConnectionId);
        var round = await _roundService.GetCurrentRoundByGameSessionIdAsync(gameGuid) ?? throw new InvalidOperationException("No round found");
        if (player.Id != round.LeaderId)
        {
            throw new InvalidOperationException("Only the leader can start voting");
        }
        var submissions = await _submissionService.GetSubmissionsByRoundIdAsync(Guid.Parse(round.Id));
        await Clients.Group(gameId).SendAsync("VotingStarted", submissions);
    }
}
