namespace ToX.DTOs;

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ToX.Models;

public class ReturnHostDtoDebug
{
    [Required]
    public long hostId { get; set; }
    [Required]
    public string hostName { get; set; }

    [Required] 
    public string hostPassword { get; set; }

    [JsonConstructor]
    public ReturnHostDtoDebug(){}

    public ReturnHostDtoDebug(Host host)
    {
        hostId = host.hostId;
        hostName = host.hostName;
        hostPassword = host.hostPassword;
    }  
}