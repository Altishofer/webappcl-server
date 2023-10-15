﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToX.Models;

namespace ToX.Repositories;

public class RoundRepository
{
    private readonly ApplicationContext _context;

    public RoundRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<List<Round>> GetAllRounds()
    {
        return _context.Round.ToList();
    }
    
    public async Task<Round?> GetRoundById(long roundId)
    {
        return await _context.Round.FindAsync(roundId);
    }
    
    public async Task<bool> RoundExistsById(long roundId)
    {
        return await _context.Round.AnyAsync(h => h.Id == roundId);
    }

    public async Task<long> NextRoundId()
    {
        return await _context.Round.AnyAsync() ? (await _context.Round.MaxAsync(u => u.Id)) + 1 : 0;
    }

    public async Task<Round> SaveRound(Round round)
    {
        _context.Round.Add(round);
        await _context.SaveChangesAsync();
        return round;
    }
}