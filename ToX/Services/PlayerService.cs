using ToX.Models;
using ToX.Repositories;

namespace ToX.Services;

public class PlayerService
{
    private readonly ApplicationContext _context;
    private readonly PlayerRepository _playerRepository;

    public PlayerService(ApplicationContext context)
    {
        _context = context;
        _playerRepository = new PlayerRepository(_context);
    }
}