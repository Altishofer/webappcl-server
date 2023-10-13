using ToX.Models;

namespace ToX.Repositories;

public class PlayerRepository
{
    private readonly ApplicationContext _context;

    public PlayerRepository(ApplicationContext context)
    {
        _context = context;
    }

    public List<Player> GetAllPlayers()
    {
        return _context.Player.ToList();
    }
}