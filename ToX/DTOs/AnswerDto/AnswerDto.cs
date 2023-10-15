using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ToX.Models;

namespace ToX.DTOs;

public class AnswerDto
{
  [Key]
  public long Id { get; set; }
    
  [Required]
  public long RoundId { get; set; }
  
  [Required]
  public long PlayerId { get; set; }
    
  [Required]
  public string AnswerTarget { get; set; }
    
  [JsonConstructor]
  public AnswerDto(){}

  public AnswerDto(Answer answer)
  {
    Id = answer.Id;
    RoundId = answer.RoundId;
    PlayerId = answer.PlayerId;
    AnswerTarget = answer.AnswerTarget;
  }
}