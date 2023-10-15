using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

    public async Task<List<Host>> GetAllHosts()
    {
        return _context.Host.ToList();
    }
    
    public async Task<Host?> GetHostByHostName(string hostName)
    {
        return await _context.Host.FirstOrDefaultAsync(h => h.hostName == hostName);
    }
    
    public async Task<bool> HostExistsByHostName(string hostName)
    {
        return await _context.Host.AnyAsync(h => h.hostName == hostName);
    }

    public async Task<long> NextHostId()
    {
        return await _context.Host.AnyAsync() ? (await _context.Host.MaxAsync(u => u.hostId)) + 1 : 0;
    }

    public async Task<Host> SaveHost(Host host)
    {
        _context.Host.Add(host);
        await _context.SaveChangesAsync();
        return host;
    }
}