using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ToX.Models;
using ToX.Services;

namespace ToX.DTOs.PlayerDto;

public class RegisterPlayerDto
{
    [Required]
    public string PlayerName { get; set; }
    
    [JsonConstructor]
    public RegisterPlayerDto(){}

    public RegisterPlayerDto(Player player)
    {
        PlayerName = player.PlayerName;
    }

    public Player toEntity()
    {
        Player player = new Player();
        player.PlayerName = PlayerName;
        return player;
    }
    
}