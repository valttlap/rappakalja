using AutoMapper;
using Sanasoppa.Core.DTOs;
using Sanasoppa.Core.Repositories;
using Sanasoppa.Model.Entities;

namespace Sanasoppa.Core.Services;

public class VoteService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VoteService(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<VoteDto> CreateAsync(VoteDto voteDto)
    {
        var vote = _mapper.Map<Vote>(voteDto);
        var voteEntity = await _unitOfWork.VoteRepository.CreateAsync(vote);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<VoteDto>(voteEntity);
    }

    public async Task<IEnumerable<VoteDto>> GetVotesByRoundIdAsync(Guid roundId)
    {
        var votes = await _unitOfWork.VoteRepository.GetVotesByRoundIdAsync(roundId);
        return _mapper.Map<IEnumerable<VoteDto>>(votes);
    }

    public async Task<IEnumerable<VoteDto>> GetVotesBySubmissionIdAsync(Guid submissionId)
    {
        var votes = await _unitOfWork.VoteRepository.GetVotesBySubmissionIdAsync(submissionId);
        return _mapper.Map<IEnumerable<VoteDto>>(votes);
    }

    public async Task<IEnumerable<VoteDto>> GetVotesByPlayerIdAsync(Guid playerId)
    {
        var votes = await _unitOfWork.VoteRepository.GetVotesByPlayerIdAsync(playerId);
        return _mapper.Map<IEnumerable<VoteDto>>(votes);
    }
}
