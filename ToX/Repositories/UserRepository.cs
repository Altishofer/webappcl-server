using ToX.Models;

namespace ToX.Repositories;

public class UserRepository
{
    private readonly ApplicationContext _context;

    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }

    public List<User> GetAllUsers()
    {
        return _context.User.ToList();
    }
}