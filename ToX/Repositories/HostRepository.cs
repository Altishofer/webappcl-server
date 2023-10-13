using ToX.Models;
using Host = ToX.Models.Host;

namespace ToX.Repositories;

public class HostRepository
{
    private readonly ApplicationContext _context;

    public HostRepository(ApplicationContext context)
    {
        _context = context;
    }

    public List<Host> GetAllHosts()
    {
        return _context.Host.ToList();
    }
}