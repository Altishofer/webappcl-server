using System.Collections.Generic;
using System.Threading.Tasks;
using ToX.DTOs;
using ToX.Models;
using ToX.Repositories;

namespace ToX.Services;

public class AnswerService
{
  private readonly ApplicationContext _context;
  private readonly AnswerRepository _answerRepository;

  public AnswerService(ApplicationContext context)
  {
    _context = context;
    _answerRepository = new AnswerRepository(_context);
  }
    
  public async Task<List<Answer>> GetAllAnswers()
  {
    return await _answerRepository.GetAllAnswers();
  }
  
  public async Task<Answer> CreateAnswer(AnswerDto answerDto)
  {
    Answer answer = new Answer();
    answer.Id = await _answerRepository.NextAnswerId();
    answer.RoundId = answerDto.RoundId;
    return await _answerRepository.SaveAnswer(answer);
  }
    
  public async Task<Answer?> GetAnswerOrNull(long id)
  {
    return await _answerRepository.GetAnswerById(id);
  }
}