namespace ToX.DTOs;

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ToX.Models;

public class ReturnUserDtoDebug
{
    [Required]
    public long userId { get; set; }
    [Required]
    public String userName { get; set; }

    [Required] 
    public String userPassword { get; set; }

    [JsonConstructor]
    public ReturnUserDtoDebug(){}

    public ReturnUserDtoDebug(User user)
    {
        userId = user.userId;
        userName = user.userName;
        userPassword = user.userPassword;
    }  
}