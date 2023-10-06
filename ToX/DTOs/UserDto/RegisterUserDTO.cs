using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ToX.Models;

namespace ToX.DTOs;

public class RegisterUserDTO
{
    [Required]
    public String userName { get; set; }
    [Required]
    public String userPassword { get; set; }
    
    [JsonConstructor]
    public RegisterUserDTO(){}

    public RegisterUserDTO(User user)
    {
        userName = user.userName;
        userPassword = user.userPassword;
    }
    
    public RegisterUserDTO(String userName, String userPassword)
    {
        this.userName = userName;
        this.userPassword = userPassword;
    }
    
    public User toEntity()
    {
        User user = new User();
        user.userPassword = userPassword;
        user.userName = userName;
        
        return user;
    }
}