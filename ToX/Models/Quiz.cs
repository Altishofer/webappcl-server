using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ToX.Models;

[Index(nameof(Id), IsUnique = true)]
public class Quiz
{
    [Key]
    public long Id { get; set; }
    
    [Required]
    public long HostId { get; set; }
    
}