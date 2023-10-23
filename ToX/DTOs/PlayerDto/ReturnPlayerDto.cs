using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ToX.Models;
using ToX.Services;

namespace ToX.DTOs.PlayerDto;

public class ReturnPlayerDto
{
    [Required]
    public long Id { get; set; }
    
    [Required]
    public string PlayerName { get; set; }
    
    [Required]
    public long QuizId { get; set; }
    
    [JsonConstructor]
    public ReturnPlayerDto(){}

    public ReturnPlayerDto(Player player)
    {
        Id = player.Id;
        PlayerName = player.PlayerName;
        QuizId = player.QuizId;
    }

    public Player toEntity()
    {
        return new Player(Id, PlayerName, QuizId);
    }
    
}