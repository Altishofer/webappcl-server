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
    
  [JsonConstructor]
  public RoundDto(){}

  public RoundDto(Round round)
  {
    Id = round.Id;
    QuizId = round.QuizId;
  }
}