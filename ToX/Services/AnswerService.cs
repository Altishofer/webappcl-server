using System.Collections.Generic;
using System.Threading.Tasks;
using ToX.DTOs;
using ToX.Models;
using ToX.Repositories;
using Word2vec.Tools;

namespace ToX.Services;

public class AnswerService
{
  private readonly ApplicationContext _context;
  private readonly AnswerRepository _answerRepository;
  private readonly Word2VectorService _word2VectorService;

  public AnswerService(ApplicationContext context, Word2VectorService word2VectorService)
  {
    _context = context;
    _answerRepository = new AnswerRepository(_context);
    _word2VectorService = word2VectorService;
  }
    
  public async Task<List<Answer>> GetAllAnswers()
  {
    return await _answerRepository.GetAllAnswers();
  }
  
  public async Task<Answer> CreateAnswer(AnswerDto answerDto, float[] target)
  {
    Representation targetRepresentation = new Representation("target", target);
    Answer answer = answerDto.toAnswer();
    answer.Id = await _answerRepository.NextAnswerId();
    answer.AnswerTarget = await _word2VectorService.WordCalculation(answerDto.Additions, answerDto.Subtractions);
    answer.Distance = await _word2VectorService.FindDistance(answer.AnswerTarget[0], targetRepresentation);
    answer.Points = (long) ((1 + answer.Distance) / 2 * 100);
    return await _answerRepository.SaveAnswer(answer);
  }
    
  public async Task<Answer?> GetAnswerOrNull(long id)
  {
    return await _answerRepository.GetAnswerById(id);
  }
  
  public async Task<bool> AnswerExists(string playerName, long roundId)
  {
    return await _answerRepository.AnswerExistsByPlayerRound(playerName, roundId);
  }
  
  public async Task<List<Answer>> GetAnswersByRoundId(long id)
  {
    return await _answerRepository.GetAnswerByRoundId(id);
  }
}