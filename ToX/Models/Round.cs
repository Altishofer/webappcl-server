using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace ToX.Models;

[Index(nameof(Id), IsUnique = true)]
public class Round
{
    [Key]
    public long Id { get; set; }
    
    [Required]
    public long QuizId { get; set; }

    [Required]
    public WordVector RoundTarget { get; set; }
    
    public String[] ForbiddenWords { get; set; }
    
    // QuizController -> addRound(RoundObj), addQuiz(Quiz), AnswerRound(Answer), all getters(ID), getResults(Round), getResults(Quiz)
    
}