using ToX.Models;

namespace ToX.Repositories;

public class RoundRepository
{
    private readonly ApplicationContext _context;

    public RoundRepository(ApplicationContext context)
    {
        _context = context;
    }

    public List<Round> GetAllRounds()
    {
        return _context.Round.ToList();
    }
}