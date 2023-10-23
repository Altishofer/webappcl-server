using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToX.Models;

namespace ToX.Repositories;

public class PlayerRepository
{
    private readonly ApplicationContext _context;

    public PlayerRepository(ApplicationContext context)
    {
        _context = context;
    }
    public async Task<List<Player>> GetAllPlayers()
    {
        return _context.Player.ToList();
    }
    
    public async Task<Player?> GetPlayer(string playerName, long quizId)
    {
        return await _context.Player.FirstOrDefaultAsync(h => h.PlayerName == playerName && h.QuizId == quizId);
    }
    
    public async Task<bool> PlayerExists(string playerName, long quizId)
    {
        return await _context.Player.AnyAsync(h => h.PlayerName == playerName && h.QuizId == quizId);
    }
    
    public async Task<long> NextPlayerId()
    {
        return await _context.Player.AnyAsync() ? (await _context.Player.MaxAsync(u => u.Id)) + 1 : 0;
    }

    public async Task<Player> SavePlayer(Player player)
    {
        _context.Player.Add(player);
        await _context.SaveChangesAsync();
        return player;
    }
}