namespace ToX.Services;

using System;
using System.Threading.Tasks;
using ToX.Models;
using Microsoft.EntityFrameworkCore;

public class UserService
{
    private readonly ApplicationContext _context;

    public UserService(ApplicationContext context)
    {
        _context = context;
    }
}
