using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ToX.Models;

namespace ToX.DTOs.QuizDto;

public class QuizDto
{
    [Key]
    public long Id { get; set; }
    
    [Required]
    public long HostId { get; set; }
    
    [JsonConstructor]
    public QuizDto(){}

    public QuizDto(Quiz quiz)
    {
        Id = quiz.Id;
        HostId = quiz.HostId;
    }
}