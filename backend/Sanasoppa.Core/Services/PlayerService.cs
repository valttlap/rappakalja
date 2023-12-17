using AutoMapper;
using Sanasoppa.Core.DTOs;
using Sanasoppa.Core.Exceptions;
using Sanasoppa.Core.Repositories;
using Sanasoppa.Model.Entities;

namespace Sanasoppa.Core.Services;

public class PlayerService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PlayerService(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PlayerDto> CreatePlayerAsync(PlayerDto playerDto)
    {
        var player = _mapper.Map<Player>(playerDto);
        var createdPlayer = await _unitOfWork.PlayerRepository.CreateAsync(player);
        if (await _unitOfWork.GameRepository.GetOwnerAsync(player.GameSessionId) is null)
        {
            await _unitOfWork.GameRepository.SetOwnerAsync(player.GameSessionId, player.Id);
        }
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<PlayerDto>(createdPlayer);
    }

    public async Task<IEnumerable<PlayerDto>> GetAllPlayersAsync()
    {
        var players = await _unitOfWork.PlayerRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<PlayerDto>>(players);
    }

    public async Task<PlayerDto> GetPlayerByIdAsync(Guid id)
    {
        var player = await _unitOfWork.PlayerRepository.GetByIdAsync(id);
        return _mapper.Map<PlayerDto>(player);
    }

    public async Task<PlayerDto> GetPlayerByConnectionIdAsync(string connectionId)
    {
        var player = await _unitOfWork.PlayerRepository.GetByConnectionIdAsync(connectionId) ?? throw new NotFoundException($"Player with connection id {connectionId} not found");
        return _mapper.Map<PlayerDto>(player);
    }

    public async Task DeletePlayerByIdAsync(Guid id)
    {
        await _unitOfWork.PlayerRepository.DeleteByIdAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}
