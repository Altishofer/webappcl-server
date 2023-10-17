using System.Collections.Generic;
using System.Threading.Tasks;
using ToX.DTOs.RoundDto;
using ToX.Models;
using ToX.Repositories;

namespace ToX.Services;

public class RoundService
{
  private readonly ApplicationContext _context;
  private readonly RoundRepository _roundRepository;
  private readonly Word2VectorService _word2VectorService;

  public RoundService(ApplicationContext context, Word2VectorService word2VectorService)
  {
    _context = context;
    _roundRepository = new RoundRepository(_context);
    _word2VectorService = word2VectorService;
  }
    
  public async Task<List<Round>> GetAllRounds()
  {
    return await _roundRepository.GetAllRounds();
  }
  
  public async Task<List<Round>> GetAllRoundsByQuiz(Quiz quiz)
  {
    return await _roundRepository.GetRoundsByQuizId(quiz.Id);
  }
  
  public async Task<List<Round>> GetAllRoundsByQuiz(long id)
  {
    return await _roundRepository.GetRoundsByQuizId(id);
  }
  
  public async Task<Round> CreateRound(RoundDto roundDto)
  {
    Round round = roundDto.toRound();
    round.Id = await _roundRepository.NextRoundId();
    return await _roundRepository.SaveRound(round);
  }
    
  public async Task<Round?> GetRoundOrNull(long id)
  {
    return await _roundRepository.GetRoundById(id);
  }
}