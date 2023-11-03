using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ToX.Models;

namespace ToX.DTOs.ResultDto;

public class FinalResultDto
{
  [Key]
  public long Id { get; set; }
    
  [Microsoft.Build.Framework.Required]
  public string PlayerName { get; set; }
    
  [Microsoft.Build.Framework.Required]
  public long Points { get; set; }
    

  [JsonConstructor]
  public FinalResultDto(){}

  public FinalResultDto(string playerName, long points)
  {
    PlayerName = playerName;
    Points = points;
  }
}