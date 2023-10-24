using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ToX.Models;

namespace ToX.DTOs.socketDto;

public class SocketPlayerDto
{
  [Required]
  public long Id { get; set; }
    
  [Required]
  public string PlayerName { get; set; }
    
  [JsonConstructor]
  public SocketPlayerDto(){}

  public SocketPlayerDto(Player player)
  {
    Id = player.Id;
    PlayerName = player.PlayerName;
  }

  public Player toEntity()
  {
    return new Player(Id, PlayerName);
  }
    
}