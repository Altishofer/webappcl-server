using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ToX.Models;

[Index(nameof(Id), IsUnique = true)]
public class Answer
{
    [Key]
    public long Id { get; set; }
    
    [Required]
    public long RoundId { get; set; }
    
    [Required]
    public long PlayerId { get; set; }
    
    [Required]
    public string AnswerTarget { get; set; }
    
    public Answer(){}

    public Answer(long roundId, long playerId, string answerTarget)
    {
        RoundId = roundId;
        PlayerId = playerId;
        AnswerTarget = answerTarget;
    }
}