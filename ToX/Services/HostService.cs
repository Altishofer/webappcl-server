using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ToX.DTOs;
using ToX.DTOs.PlayerDto;
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
        _hostRepository = new HostRepository(_context);
    }

    public async Task<bool> HostExistsByHostName(string hostName)
    {
        return await _hostRepository.HostExistsByHostName(hostName);
    }
    
    public async Task<Host> CreateHost(RegisterHostDTO hostDto)
    {
        Host host = hostDto.toEntity();
        host.hostId = await _hostRepository.NextHostId();
        return await _hostRepository.SaveHost(host);
    }
    
    public async Task<Host?> GetHostOrNull(RegisterHostDTO hostDto)
    {
        return await _hostRepository.GetHostByHostName(hostDto.hostName);
    }
    
    public async Task<List<Host>> GetAllHosts()
    {
        return await _hostRepository.GetAllHosts();
    }
}
