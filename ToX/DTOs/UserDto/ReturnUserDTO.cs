using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ToX.Models;

namespace ToX.DTOs;

public class ReturnUserDTO
{
    [Required]
    public long userId { get; set; }
    [Required]
    public String userName { get; set; }
    
    [JsonConstructor]
    public ReturnUserDTO(){}

    public ReturnUserDTO(User user)
    {
        userId = user.userId;
        userName = user.userName;
    }
}