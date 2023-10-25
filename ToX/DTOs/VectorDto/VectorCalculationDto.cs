using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace ToX.DTOs.VectorDto;

public class VectorCalculationDto
{

  public List<string>? Additions { get; set; }

  public List<string>? Subtractions { get; set; }
  
  [JsonConstructor]
  public VectorCalculationDto(){}

  public VectorCalculationDto(List<string>? additions, List<string>? subtractions)
  {
    Additions = additions;
    Subtractions = subtractions;
  }
}