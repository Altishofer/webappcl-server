using System.ComponentModel.DataAnnotations;
using ToX.Services;

namespace ToX.Models;

public class Player
{
    [Key] 
    public long Id;

    [Required] 
    public string PlayerName;

    public Player()
    {
    }

    public Player(long id, string playerName)
    {
        Id = id;
        PlayerName = playerName;
    }
}