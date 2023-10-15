using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToX.Models;

[Index(nameof(Id), IsUnique = true)]
public class Answer
{
  [Key]
  [Column("id")]
  public long Id { get; set; }
    
  [Required]
  [Column("roundid")]
  public long RoundId { get; set; }
    
  [Required]
  [Column("playerid")]
  public long PlayerId { get; set; }
    
  [Required]
  [Column("answertarget")]
  public string AnswerTarget { get; set; }
    
  public Answer(){}

  public Answer(long roundId, long playerId, string answerTarget)
  {
    RoundId = roundId;
    PlayerId = playerId;
    AnswerTarget = answerTarget;
  }
}