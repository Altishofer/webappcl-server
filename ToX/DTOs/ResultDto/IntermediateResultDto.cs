using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ToX.Models;

namespace ToX.DTOs.ResultDto;

public class IntermediateResultDto
{
  [Key]
  public long Id { get; set; }
  
  [Required]
  public string PlayerName { get; set; }
  
  [Required]
  public long Points { get; set; }
  
  [Required]
  public List<string> AnswerTarget { get; set; }
  
    
  [JsonConstructor]
  public IntermediateResultDto(){}

  public IntermediateResultDto(Answer answer)
  {
    PlayerName = answer.PlayerName;
    Points = answer.Points;
    AnswerTarget = answer.AnswerTarget;
  }
}