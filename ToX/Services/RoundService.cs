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

  public RoundService(ApplicationContext context)
  {
    _context = context;
    _roundRepository = new RoundRepository(_context);
  }
    
  public async Task<List<Round>> GetAllRounds()
  {
    return await _roundRepository.GetAllRounds();
  }
  
  public async Task<Round> CreateRound(RoundDto roundDto)
  {
    Round round = new Round();
    round.Id = await _roundRepository.NextRoundId();
    round.QuizId = roundDto.QuizId;
    return await _roundRepository.SaveRound(round);
  }
    
  public async Task<Round?> GetRoundOrNull(long id)
  {
    return await _roundRepository.GetRoundById(id);
  }
}