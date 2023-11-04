using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using ToX.Models;

namespace ToX.DTOs;

public class AnswerDto
{
  [Key]
  public long Id { get; set; }
  
  [Required]
  public long QuizId { get; set; }
    
  [Required]
  public long RoundId { get; set; }
  
  [Required]
  public string PlayerName { get; set; }
  
  [Required]
  public List<string> Subtractions { get; set; }
  
  [Required]
  public List<string> Additions { get; set; }
    
  [JsonConstructor]
  public AnswerDto(){}

  public AnswerDto(Answer answer)
  {
    Id = answer.Id;
    QuizId = answer.QuizId;
    RoundId = answer.RoundId;
    PlayerName = answer.PlayerName;
    Subtractions = answer.Subtractions;
    Additions = answer.Additions;
  }
  
  public AnswerDto(long quizId, long roundId, string playerName)
  {
    QuizId = quizId;
    RoundId = roundId;
    PlayerName = playerName;
    Subtractions = new List<string>();
    Additions = new List<string>();
  }

  public Answer toAnswer()
  {
    Answer answer = new Answer();
    answer.RoundId = RoundId;
    answer.PlayerName = PlayerName;
    answer.QuizId = QuizId;
    answer.Additions = Additions;
    answer.Subtractions = Subtractions;
    return answer;
  }
}