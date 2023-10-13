using System.ComponentModel.DataAnnotations;

namespace ToX.Models;

public class Host
{
    [Required]
    public long hostId { get; set; }
    [Required]
    public String hostName { get; set; }
    [Required]
    public String hostPassword { get; set; }
    
    public Host(){}
    
    public Host(long hostId, string hostName, string hostPassword)
    {
        this.hostId = hostId;
        this.hostName = hostName;
        this.hostPassword = hostPassword;
    }
}