using Microsoft.Build.Framework;
using Newtonsoft.Json;
using ToX.Models;

namespace ToX.DTOs.QuizDto;

public class QuizRoundDto
{

  public long HostId { get; set; }
    

  public string Title { get; set; }
  
  public long QuizId { get; set; }
  
  
  public List<RoundDto.RoundDto> Rounds { get; set; }
    
  [JsonConstructor]
  public QuizRoundDto(){}

  public QuizRoundDto(Quiz quiz)
  {
    HostId = quiz.HostId;
    Title = quiz.Title;
    QuizId = quiz.Id;
    Rounds = new List<RoundDto.RoundDto>();
  }
}