using System.ComponentModel.DataAnnotations;

namespace ToX.Models;

public class Quiz
{
    [Key]
    public long Id { get; set; }
    
    [Required]
    public long HostId { get; set; }
    
}