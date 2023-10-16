using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace ToX.DTOs.VectorDto;

public class VectorCalculationDto
{

  public string?[] Additions { get; set; }

  public string?[] Subtractions { get; set; }
  
  [JsonConstructor]
  public VectorCalculationDto(){}

  public VectorCalculationDto(string?[] additions, string?[] subtractions)
  {
    Additions = additions;
    Subtractions = subtractions;
  }
}