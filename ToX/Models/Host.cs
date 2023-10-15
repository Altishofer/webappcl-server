using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToX.Models;

[Index(nameof(Host.hostId), IsUnique = true)]
public class Host
{
  [Required]
  [Column("hostid")]
  public long hostId { get; set; }
  [Required]
  [Column("hostname")]
  public string hostName { get; set; }
  [Required]
  [Column("hostpassword")]
  public string hostPassword { get; set; }
    
  public Host(){}
    
  public Host(long hostId, string hostName, string hostPassword)
  {
    this.hostId = hostId;
    this.hostName = hostName;
    this.hostPassword = hostPassword;
  }
}