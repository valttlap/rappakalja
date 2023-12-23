using AutoMapper;
using Sanasoppa.Core.DTOs;
using Sanasoppa.Core.Exceptions;
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

    public async Task<SubmissionsDoneDto> GetSubmissionsDoneAsync(Guid roundId)
    {
        var round = await _unitOfWork.RoundRepository.GetByIdAsync(roundId) ?? throw new NotFoundException($"Round with id {roundId} not found");
        var game = await _unitOfWork.GameRepository.GetGameSessionByIdAsync(round.GameSessionId) ?? throw new NotFoundException($"Game session with id {round.GameSessionId} not found");
        var submissions = await _unitOfWork.SubmissionRepository.GetSubmissionsByRoundIdAsync(roundId);
        var submissionsDone = new SubmissionsDoneDto
        {
            SubmissionsDone = submissions.Count(),
            SubmissionsTotal = game.Players.Count
        };
        return submissionsDone;
    }

    public async Task<bool> AllPlayersSubmittedAsync(Guid roundId)
    {
        var round = await _unitOfWork.RoundRepository.GetByIdAsync(roundId) ?? throw new NotFoundException($"Round with id {roundId} not found");
        var game = await _unitOfWork.GameRepository.GetGameSessionByIdAsync(round.GameSessionId) ?? throw new NotFoundException($"Game session with id {round.GameSessionId} not found");
        var submissions = await _unitOfWork.SubmissionRepository.GetSubmissionsByRoundIdAsync(roundId);
        if (game.Players.Count == submissions.Count())
        {
            return true;
        }
        return false;
    }

    public async Task<SubmissionsDoneDto> GetVotesDoneAsync(Guid roundId)
    {
        var round = await _unitOfWork.RoundRepository.GetByIdAsync(roundId) ?? throw new NotFoundException($"Round with id {roundId} not found");
        var votes = await _unitOfWork.VoteRepository.GetVotesByRoundIdAsync(roundId);
        var playersInGame = (await _unitOfWork.GameRepository.GetGameSessionByIdAsync(round.GameSessionId) ?? throw new NotFoundException($"Game session with id {round.GameSessionId} not found")).Players.Count;
        var votesDone = new SubmissionsDoneDto
        {
            SubmissionsDone = votes.Count(),
            SubmissionsTotal = playersInGame - 1
        };

        return votesDone;
    }

    public async Task<RoundDto> StartNewRoundAsync(string gameId)
    {
        var gameGuid = Guid.TryParse(gameId, out var id) ? id : throw new ArgumentException("Invalid game id");
        var round = await _unitOfWork.RoundRepository.StartNewRound(gameGuid);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<RoundDto>(round);
    }

    public async Task<RoundDto> ResetRoundAsync(Guid roundId)
    {
        var round = await _unitOfWork.RoundRepository.ResetRoundAsync(roundId);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<RoundDto>(round);
    }
}
