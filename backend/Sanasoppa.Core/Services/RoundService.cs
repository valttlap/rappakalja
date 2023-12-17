using AutoMapper;
using Sanasoppa.Core.DTOs;
using Sanasoppa.Core.Repositories;
using Sanasoppa.Model.Entities;

namespace Sanasoppa.Core.Services;

public class RoundService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RoundService(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<RoundDto> CreateAsync(RoundDto roundDto)
    {
        var round = _mapper.Map<Round>(roundDto);
        var roundEntity = await _unitOfWork.RoundRepository.CreateAsync(round);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<RoundDto>(roundEntity);
    }

    public async Task<RoundDto?> GetByIdAsync(Guid id)
    {
        var round = await _unitOfWork.RoundRepository.GetByIdAsync(id);
        return _mapper.Map<RoundDto?>(round);
    }

    public async Task<IEnumerable<RoundDto>> GetRoundsByGameSessionIdAsync(Guid gameSessionId)
    {
        var rounds = await _unitOfWork.RoundRepository.GetRoundsByGameSessionIdAsync(gameSessionId);
        return _mapper.Map<IEnumerable<RoundDto>>(rounds);
    }

    public async Task<RoundDto?> GetCurrentRoundByGameSessionIdAsync(Guid gameSessionId)
    {
        var round = await _unitOfWork.RoundRepository.GetCurrentRoundByGameSessionIdAsync(gameSessionId);
        return _mapper.Map<RoundDto?>(round);
    }

    public async Task AddWordToRoundAsync(Guid roundId, string word)
    {
        await _unitOfWork.RoundRepository.AddWordToRoundAsync(roundId, word);
        await _unitOfWork.SaveChangesAsync();
    }
}
