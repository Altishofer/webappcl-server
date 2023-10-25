using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Word2vec.Tools;

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
  [Column("subtractions")]
  public List<string> Subtractions { get; set; }
  
  [Required]
  [Column("additions")]
  public List<string> Additions { get; set; }
  
  [Required]
  [Column("answertarget")]
  public List<string> AnswerTarget { get; set; }
  
  [Required]
  [Column("distance")]
  public double Distance { get; set; }
  
  [Column("points")]
  public long Points { get; set; }
    
  public Answer(){}

  public Answer(long id, long roundId, string playerName, long quizId, List<string> additions, List<string> subtractions, List<string> answerTarget, long points, float distance)
  {
    Id = id;
    RoundId = roundId;
    PlayerName = playerName;
    QuizId = quizId;
    Subtractions = subtractions;
    Additions = additions;
    Distance = distance;
    Points = points;
    AnswerTarget = answerTarget;
  }
}