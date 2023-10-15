using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ToX.Models;
using Host = ToX.Models.Host;

namespace ToX.DTOs;

public class ReturnHostDTO
{
    [Required]
    public long userId { get; set; }
    [Required]
    public string userName { get; set; }
    
    [JsonConstructor]
    public ReturnHostDTO(){}

    public ReturnHostDTO(Host host)
    {
        userId = host.hostId;
        userName = host.hostName;
    }
}