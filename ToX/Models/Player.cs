using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ToX.Services;

namespace ToX.Models;

[Index(nameof(Id), IsUnique = true)]
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