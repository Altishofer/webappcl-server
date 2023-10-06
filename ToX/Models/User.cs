using System.ComponentModel.DataAnnotations;

namespace ToX.Models;

public class User
{
    [Required]
    public long userId { get; set; }
    [Required]
    public String userName { get; set; }
    [Required]
    public String userPassword { get; set; }
    
    public User(){}
    
    public User(long userId, string userName, string userPassword)
    {
        this.userId = userId;
        this.userName = userName;
        this.userPassword = userPassword;
    }
}