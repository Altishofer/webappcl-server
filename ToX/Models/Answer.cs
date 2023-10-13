using System.ComponentModel.DataAnnotations;

namespace ToX.Models;

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
        this.RoundId = roundId;
        this.PlayerId = playerId;
        this.AnswerTarget = answerTarget;
    }
}