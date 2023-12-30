using AutoMapper;
using Sanasoppa.Core.DTOs;
using Sanasoppa.Core.Exceptions;
using Sanasoppa.Core.Repositories;

namespace Sanasoppa.Core.Services;

public class GameService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GameService(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GameSessionDto> CreateGameSessionAsync()
    {
        var gameSession = await _unitOfWork.GameRepository.CreateGameSessionAsync();
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<GameSessionDto>(gameSession);
    }

    public async Task<IEnumerable<GameSessionDto>> GetGameSessionsAsync()
    {
        var gameSessions = await _unitOfWork.GameRepository.GetGameSessionsAsync();
        return _mapper.Map<IEnumerable<GameSessionDto>>(gameSessions);
    }

    public async Task<GameSessionDto> GetGameSessionByIdAsync(Guid id)
    {
        var gameSession = await _unitOfWork.GameRepository.GetGameSessionByIdAsync(id) ?? throw new NotFoundException($"Game session with id {id} not found");
        return _mapper.Map<GameSessionDto>(gameSession);
    }

    public async Task<GameSessionDto> GetGameSessionByJoinCodeAsync(string joinCode)
    {
        var gameSession = await _unitOfWork.GameRepository.GetGameSessionByJoinCodeAsync(joinCode) ?? throw new NotFoundException($"Game session with join code {joinCode} not found");
        return _mapper.Map<GameSessionDto>(gameSession);
    }

    public async Task<bool> HasOwnerAsync(string gameId)
    {
        var gameGuid = Guid.Parse(gameId);
        return (await _unitOfWork.GameRepository.GetOwnerAsync(gameGuid)) is not null;
    }

    public async Task SetOwnerAync(string gameId, string playerId)
    {
        var gameGuid = Guid.Parse(gameId);
        var playerGuid = Guid.Parse(playerId);

        await _unitOfWork.GameRepository.SetOwnerAsync(gameGuid, playerGuid);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<PlayerDto>> GetPlayersByGameSessionIdAsync(Guid gameSessionId)
    {
        var game = await _unitOfWork.GameRepository.GetGameSessionByIdAsync(gameSessionId) ?? throw new NotFoundException($"Game session with id {gameSessionId} not found");
        return _mapper.Map<IEnumerable<PlayerDto>>(game.Players);
    }

    public async Task StartGameSessionAsync(Guid id)
    {
        await _unitOfWork.GameRepository.StartGameAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}
