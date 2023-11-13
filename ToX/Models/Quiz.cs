using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToX.Models;

[Index(nameof(Id), IsUnique = true)]
public class Quiz
{
  [Key]
  [Column("id")]
  public long Id { get; set; }
    
  [Required]
  [Column("hostid")]
  public long HostId { get; set; }
    
  [Required]
  [Column("title")]
  public string Title { get; set; }
  
  public Quiz()
  {
  }

  public Quiz(long id, long hostId, string title)
  {
    Id = id;
    HostId = hostId;
    Title = title;
  }
    
}
