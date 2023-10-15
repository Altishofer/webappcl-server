using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ToX.Models;
using Host = ToX.Models.Host;

namespace ToX.DTOs;

public class RegisterHostDTO
{
    [Required]
    public string hostName { get; set; }
    [Required]
    public string hostPassword { get; set; }
    
    [JsonConstructor]
    public RegisterHostDTO(){}

    public RegisterHostDTO(Host host)
    {
        hostName = host.hostName;
        hostPassword = host.hostPassword;
    }
    
    public RegisterHostDTO(string hostName, string hostPassword)
    {
        this.hostName = hostName;
        this.hostPassword = hostPassword;
    }
    
    public Host toEntity()
    {
        Host host = new Host();
        host.hostPassword = hostPassword;
        host.hostName = hostName;
        
        return host;
    }
}