using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Sanasoppa.API.Extensions;
using Sanasoppa.Core.DTOs;
using Sanasoppa.Core.Services;

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

    public override async Task OnConnectedAsync()
    {
        var user = Context.User ?? throw new InvalidOperationException("User is null");
        var userId = user.GetUserId() ?? throw new InvalidDataException("User ID is null");
        if (await _playerService.PlayerExistsAsync(userId))
        {
            await _playerService.UpdatePlayerConnectionIdAsync(userId, Context.ConnectionId);
        }
    }

    public async Task<string> JoinGame(string joinCode)
    {
        var user = Context.User ?? throw new InvalidOperationException("User is null");
        var userId = user.GetUserId() ?? throw new InvalidDataException("User ID is null");
        var username = user.GetUsername() ?? throw new InvalidDataException("Username is null");
        joinCode = joinCode.ToLower();
        var gameSession = await _gameService.GetGameSessionByJoinCodeAsync(joinCode);
        PlayerDto player;
        if (await _playerService.PlayerExistsAsync(userId))
        {
            player = await _playerService.GetPlayerByPlayerIdAsync(userId);
            await _playerService.UpdatePlayerGame(userId, gameSession.Id);
        }
        else
        {
            player = new PlayerDto()
            {
                Id = Guid.NewGuid().ToString(),
                Name = username,
                ConnectionId = Context.ConnectionId,
                GameSessionId = gameSession.Id,
            };
            await _playerService.CreatePlayerAsync(player);
        }
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

    public async Task<GameStatusDto> GetGameStatus()
    {
        var user = ValidateCurrentUser();
        var player = await _playerService.GetPlayerByPlayerIdAsync(user.GetUserId()!);
        var gameSession = await GetGameSessionAsync(player);
        var currentRound = await GetCurrentRoundAsync(gameSession) ?? throw new ApplicationException("Round not found or game not started");
        var isLeader = IsLeader(currentRound, player);

        return await DetermineGameStatus(player, currentRound, isLeader);
    }

    private ClaimsPrincipal ValidateCurrentUser()
    {
        var user = Context.User ?? throw new InvalidDataException("User is null");
        if (user.GetUserId() is null)
        {
            throw new InvalidDataException("User id is null");
        }
        return user;
    }

    private async Task<GameSessionDto> GetGameSessionAsync(PlayerDto player)
    {
        return await _gameService.GetGameSessionByIdAsync(Guid.Parse(player.GameSessionId));
    }

    private async Task<RoundDto?> GetCurrentRoundAsync(GameSessionDto gameSession)
    {
        return await _roundService.GetCurrentRoundByGameSessionIdAsync(Guid.Parse(gameSession.Id));
    }

    private bool IsLeader(RoundDto currentRound, PlayerDto player)
    {
        return currentRound?.LeaderId == player.Id;
    }

    private async Task<GameStatusDto> DetermineGameStatus(PlayerDto player, RoundDto currentRound, bool isLeader)
    {
        var gameStatus = new GameStatusDto();

        if (currentRound?.Word is null)
        {
            gameStatus.State = isLeader ? GameState.SubmitWord : GameState.Wait;
        }
        else
        {
            await ProcessRoundWithWord(gameStatus, player, currentRound, isLeader);
        }

        return gameStatus;
    }

    private async Task ProcessRoundWithWord(GameStatusDto gameStatus, PlayerDto player, RoundDto currentRound, bool isLeader)
    {
        gameStatus.Word = currentRound.Word;

        if (await _roundService.AllPlayersSubmittedAsync(Guid.Parse(currentRound.Id)))
        {
            await ProcessAllPlayersSubmitted(gameStatus, currentRound, isLeader);
        }
        else
        {
            await ProcessPendingSubmissions(gameStatus, player, currentRound);
        }
    }

    private async Task ProcessAllPlayersSubmitted(GameStatusDto gameStatus, RoundDto currentRound, bool isLeader)
    {
        if (isLeader)
        {
            var submissions = await _submissionService.GetSubmissionsByRoundIdAsync(Guid.Parse(currentRound.Id));
            submissions = submissions.Scramble();
            gameStatus.State = GameState.ReadGuesses;
            gameStatus.Submissions = submissions;
        }
        else
        {
            gameStatus.State = GameState.Wait;
        }
    }

    private async Task ProcessPendingSubmissions(GameStatusDto gameStatus, PlayerDto player, RoundDto currentRound)
    {
        gameStatus.State = await _submissionService.HasPlayerSubmittedAsync(player.Id, currentRound.Id)
            ? GameState.Wait
            : GameState.SubmitGuess;
    }

}
