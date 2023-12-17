using Microsoft.AspNetCore.SignalR;
using Sanasoppa.Core.DTOs;
using Sanasoppa.Core.Services;

namespace Sanasoppa.API.Hubs;

public class GameHub : Hub
{
    private readonly GameService _gameService;
    private readonly PlayerService _playerService;
    private readonly RoundService _roundService;

    public GameHub(GameService gameService, PlayerService playerService, RoundService roundService)
    {
        _gameService = gameService;
        _playerService = playerService;
        _roundService = roundService;
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }


    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
    public async Task JoinGame(string joinCode, string name)
    {
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
}
