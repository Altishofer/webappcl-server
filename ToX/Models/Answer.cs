using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToX.Models;

[Index(nameof(Id), IsUnique = true)]
public class Answer
{
  [Key]
  [Column("id")]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public long Id { get; set; }
    
  [Required]
  [Column("roundid")]
  public long RoundId { get; set; }
  
  [Required]
  [Column("quizid")]
  public long QuizId { get; set; }
    
  [Required]
  [Column("playername")]
  public string PlayerName { get; set; }
    
  [Required]
  [Column("answertarget")]
  public string AnswerTarget { get; set; }
  
  [Required]
  [Column("subtractions")]
  public string[] Subtractions { get; set; }
  
  [Required]
  [Column("additions")]
  public string[] Additions { get; set; }
    
  public Answer(){}

  public Answer(long id, long roundId, string playerName, string answerTarget, long quizId, string[] additions, string[] subtractions)
  {
    Id = id;
    RoundId = roundId;
    PlayerName = playerName;
    AnswerTarget = answerTarget;
    QuizId = quizId;
    Subtractions = subtractions;
    Additions = additions;
  }
}