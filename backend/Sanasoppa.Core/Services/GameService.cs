using AutoMapper;
using Sanasoppa.Core.DTOs;
using Sanasoppa.Core.Exceptions;
using Sanasoppa.Core.Repositories;
using Sanasoppa.Model.Entities;

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

    public async Task AddPlayerToGameSessionAsync(Guid gameSessionId, PlayerDto playerDto)
    {
        var player = _mapper.Map<Player>(playerDto);
        try
        {
            await _unitOfWork.GameRepository.AddPlayerToGameSessionAsync(gameSessionId, player);
        }
        catch (AlreadyInGameException)
        {
            throw;
        }
        catch (NotFoundException)
        {
            throw;
        }
        await _unitOfWork.SaveChangesAsync();
    }
}
