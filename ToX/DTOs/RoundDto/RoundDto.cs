using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ToX.Models;

namespace ToX.DTOs.RoundDto;

public class RoundDto
{
  [Key]
  public long Id { get; set; }
    
  [Required]
  public long QuizId { get; set; }
  
  [Required]
  public string RoundTarget { get; set; }
  
  public string[] ForbiddenWords { get; set; }
    
  [JsonConstructor]
  public RoundDto(){}

  public RoundDto(Round round)
  {
    Id = round.Id;
    QuizId = round.QuizId;
    RoundTarget = round.RoundTarget;
    ForbiddenWords = round.ForbiddenWords;
  }

  public Round toRound()
  {
    Round round = new Round();
    round.QuizId = QuizId;
    round.ForbiddenWords = ForbiddenWords;
    round.RoundTarget = RoundTarget;
    return round;
  }
}