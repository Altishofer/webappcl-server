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
  
  public string AnswerTarget { get; set; }
  
  [Required]
  public string[] Subtractions { get; set; }
  
  [Required]
  public string[] Additions { get; set; }
    
  [JsonConstructor]
  public AnswerDto(){}

  public AnswerDto(Answer answer)
  {
    Id = answer.Id;
    QuizId = answer.QuizId;
    RoundId = answer.RoundId;
    PlayerName = answer.PlayerName;
    AnswerTarget = answer.AnswerTarget;
    Subtractions = answer.Subtractions;
    Additions = answer.Additions;
  }

  public Answer toAnswer()
  {
    Answer answer = new Answer();
    answer.RoundId = RoundId;
    answer.AnswerTarget = AnswerTarget;
    answer.PlayerName = PlayerName;
    answer.QuizId = QuizId;
    answer.Additions = Additions;
    answer.Subtractions = Subtractions;
    return answer;
  }
}