﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
  public string RoundTarget { get; set; }
  
  [Required]
  [Column("roundtargetvector")]
  public float[] RoundTargetVector { get; set; }
    
  [Column("forbiddenwords")]
  public List<string> ForbiddenWords { get; set; }
  
  public Round(){}

  public Round(long id, long quizId, string roundTarget, List<string> forbiddenWords)
  {
    Id = id;
    QuizId = quizId;
    RoundTarget = roundTarget;
    ForbiddenWords = forbiddenWords;
  }
}