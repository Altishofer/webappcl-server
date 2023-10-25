using Microsoft.Build.Framework;
using Newtonsoft.Json;

namespace ToX.DTOs.ResultDto;

public class WaitResultDto
{
    
  [Required]
  public List<string> AnsweredPlayerName { get; set; }
    
  [Required]
  public List<string> NotAnsweredPlayerName { get; set; }
  
  [JsonConstructor]
  public WaitResultDto(){}

  public WaitResultDto(List<string> notAnsweredPlayerName, List<string> answeredPlayerName)
  {
    AnsweredPlayerName = answeredPlayerName;
    NotAnsweredPlayerName = notAnsweredPlayerName;
  }
}