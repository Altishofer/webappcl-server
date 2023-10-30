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
    
    public void DeleteRound(Round round)
    {
        _context.Remove(round);
    }

    public async Task<List<Round>> GetAllRounds()
    {
        return _context.Round.ToList();
    }
    
    public async Task<List<Round>> GetRoundsByQuizId(long quizId)
    {
        return _context.Round.Where(h => h.QuizId == quizId).ToList();
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

    public async Task<Round> ChangeExistingRound(Round round)
    {
        Round foundRound = await this.GetRoundById(round.Id);
        _context.Entry(foundRound).CurrentValues.SetValues(round.Id);
        _context.Entry(foundRound).Property(x => x.Id).IsModified = false;
        await _context.SaveChangesAsync();
        return round;
    }
}