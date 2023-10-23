using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ToX.Services;

namespace ToX.Models;

[Index(nameof(Id), IsUnique = true)]
[Table("player")]
public class Player
{
  [Key] 
  [Column("id")]
  public long Id { get; set; }

  [Required] 
  [Column("playername")]
  public string PlayerName { get; set; }

  [Required] 
  [Column("quizid")]
  public long QuizId { get; set; }

  public Player()
  {
  }

  public Player(long id, string playerName, long quizId)
  {
    Id = id;
    PlayerName = playerName;
    QuizId = quizId;
  }
}