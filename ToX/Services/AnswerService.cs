using System.Collections.Generic;
using System.Threading.Tasks;
using ToX.DTOs;
using ToX.DTOs.RoundDto;
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
  
  public async Task<Answer> CreateAnswer(AnswerDto answerDto, Round round)
  {
    Representation targetRepresentation = new Representation(round.RoundTarget, round.RoundTargetVector);
    Answer answer = answerDto.toAnswer();
    answer.Id = await _answerRepository.NextAnswerId();
    List<string> wordsToRemove = new List<string>();
    answerDto.Additions = answerDto.Additions.ConvertAll(x => x.ToLower());
    answerDto.Subtractions = answerDto.Subtractions.ConvertAll(x => x.ToLower());
    foreach (string wordAdd in answerDto.Additions)
    {
      if (wordAdd == "" || round.ForbiddenWords.Any(s => wordAdd.Contains(s)) || wordAdd.Contains(round.RoundTarget))
      {
        wordsToRemove.Add(wordAdd);
      }
    }
    foreach (string wordSub in answerDto.Subtractions)
    {
      if (wordSub == "" || round.ForbiddenWords.Any(s => wordSub.Contains(s)) || wordSub.Contains(round.RoundTarget))
      {
        wordsToRemove.Add(wordSub);
      }
    }
    foreach (string word in wordsToRemove)
    {
      answerDto.Additions.RemoveAll(x => x == word);
      answerDto.Subtractions.RemoveAll(x => x == word);
    }
    
    if (answerDto.Additions.Count == 0 && answerDto.Subtractions.Count == 0)
    {
      answer.AnswerTarget = new List<string>(){""};
      answer.Distance = -1;
      answer.Points = 0;
      return await _answerRepository.SaveAnswer(answer);
    }
    answer.AnswerTarget = await _word2VectorService.WordCalculation(answerDto.Additions, answerDto.Subtractions);
    answer.AnswerTarget = answer.AnswerTarget.Take(3).ToList();
    if (answer.AnswerTarget.Count == 0)
    {
      answer.AnswerTarget = new List<string>(){""};
      answer.Distance = -1;
      answer.Points = 0;
      return await _answerRepository.SaveAnswer(answer);
    }
    answer.Distance = await _word2VectorService.FindDistance(answer.AnswerTarget[0], targetRepresentation);
    if (answer.Distance < -0.99)
    {
      answer.Points = 0;
    }
    else
    {
      answer.Points = (long) ((1 + answer.Distance) / 2 * 100);

    }
    return await _answerRepository.SaveAnswer(answer);
  }
    
  public async Task<Answer?> GetAnswerOrNull(long id)
  {
    return await _answerRepository.GetAnswerById(id);
  }
  
  public void Delete(Answer answer)
  {
    _answerRepository.DeleteAnswer(answer);
  }
  
  public async Task<int> DeleteAnswerByQuiz(Quiz quiz)
  {
    return await _answerRepository.DeleteAnswersByQuiz(quiz);
  }
  
  public async Task<bool> AnswerExists(string playerName, long roundId)
  {
    return await _answerRepository.AnswerExistsByPlayerRound(playerName, roundId);
  }
  
  public async Task<List<Answer>> GetAnswersByRoundId(long id)
  {
    return await _answerRepository.GetAnswerByRoundId(id);
  }
  
  public async Task<List<Answer>> GetAnswersByPlayer(Player player)
  {
    return await _answerRepository.GetAnswerByPlayer(player);
  }
}