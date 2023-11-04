namespace ToX.DTOs.ResultDto;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ToX.Models;

public class FullResultDto
{
  [Key]
  public long Id { get; set; }
  
  [Required]
  public long Rank { get; set; }
  
  [Required]
  public string PlayerName { get; set; }
  
  [Required]
  public long LastRoundPoints { get; set; }
  
  [Required]
  public long TotalPoints { get; set; }
  
  [Required]
  public List<string> LastRoundAnswerTarget { get; set; }
  
    
  [JsonConstructor]
  public FullResultDto(){}

  public FullResultDto(Answer answer)
  {
    PlayerName = answer.PlayerName;
    LastRoundPoints = answer.Points;
    LastRoundAnswerTarget = answer.AnswerTarget;
  }
}