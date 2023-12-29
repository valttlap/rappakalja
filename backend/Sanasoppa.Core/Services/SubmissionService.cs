using AutoMapper;
using Sanasoppa.Core.DTOs;
using Sanasoppa.Core.Repositories;
using Sanasoppa.Model.Entities;

namespace Sanasoppa.Core.Services;

public class SubmissionService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SubmissionService(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SubmissionDto> CreateAsync(SubmissionDto submissionDto)
    {
        var submission = _mapper.Map<Submission>(submissionDto);
        var submissionEntity = await _unitOfWork.SubmissionRepository.CreateAsync(submission);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<SubmissionDto>(submissionEntity);
    }

    public async Task<IEnumerable<SubmissionReturnDto>> GetSubmissionsByRoundIdAsync(Guid roundId)
    {
        var submissions = await _unitOfWork.SubmissionRepository.GetSubmissionsByRoundIdAsync(roundId);
        return _mapper.Map<IEnumerable<SubmissionReturnDto>>(submissions);
    }

    public async Task<IEnumerable<SubmissionDto>> GetNotCorrectSubmissionsByRoundIdAsync(Guid roundId)
    {
        var submissions = await _unitOfWork.SubmissionRepository.GetNotCorrectSubmissionsByRoundIdAsync(roundId);
        return _mapper.Map<IEnumerable<SubmissionDto>>(submissions);
    }

    public async Task<bool> HasPlayerSubmittedAsync(string playerId, string roundId)
    {
        return await _unitOfWork.SubmissionRepository.HasPlayerSubmittedAsync(Guid.Parse(playerId), Guid.Parse(roundId));
    }

}
