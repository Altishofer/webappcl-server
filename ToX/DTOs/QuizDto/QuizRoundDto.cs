using Microsoft.Build.Framework;
using Newtonsoft.Json;
using ToX.Models;

namespace ToX.DTOs.QuizDto;

public class QuizRoundDto
{
  [Required]
  public long HostId { get; set; }
    
  [Required]
  public string Title { get; set; }
  
  [Required]
  public List<RoundDto.RoundDto> Rounds { get; set; }
    
  [JsonConstructor]
  public QuizRoundDto(){}

  public QuizRoundDto(Quiz quiz)
  {
    HostId = quiz.HostId;
    Title = quiz.Title;
    Rounds = new List<RoundDto.RoundDto>();
  }
}