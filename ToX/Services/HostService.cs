using ToX.Repositories;

namespace ToX.Services;

using System;
using System.Threading.Tasks;
using ToX.Models;
using Microsoft.EntityFrameworkCore;

public class HostService
{
    private readonly ApplicationContext _context;
    private readonly HostRepository _hostRepository;

    public HostService(ApplicationContext context)
    {
        _context = context;
        this._hostRepository = new HostRepository(_context);
    }
}
