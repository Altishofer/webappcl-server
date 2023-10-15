using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace ToX.Models;

[Index(nameof(Id), IsUnique = true)]
public class Round
{
  [Key]
  [Column("id")]
  public long Id { get; set; }
    
  [Required]
  [Column("quizid")]
  public long QuizId { get; set; }

  [Required]
  [Column("roundtarget")]
  public WordVector RoundTarget { get; set; }
    
  [Column("forbiddenwords")]
  public string[] ForbiddenWords { get; set; }
  
}