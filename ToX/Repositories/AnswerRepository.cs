using Microsoft.EntityFrameworkCore;
using ToX.Models;

namespace ToX.Repositories;

public class AnswerRepository
{
    private readonly ApplicationContext _context;
    
    public AnswerRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<List<Answer>> GetAllAnswers()
    {
        return _context.Answer.ToList();
    }
    
    public async Task<Answer?> GetAnswerById(long answerId)
    {
        return await _context.Answer.FindAsync(answerId);
    }
    
    public async Task<bool> AnswerExistsByPlayerRound(string playerName, long roundId)
    {
        return await _context.Answer.FirstOrDefaultAsync(h => h.PlayerName == playerName && h.RoundId == roundId) != null;
    }
    
    public async Task<bool> AnswerExistsById(long answerId)
    {
        return await _context.Answer.FirstOrDefaultAsync(h => h.Id == answerId) != null;
    }

    public async Task<long> NextAnswerId()
    {
        return await _context.Answer.AnyAsync() ? (await _context.Answer.MaxAsync(u => u.Id)) + 1 : 0;
    }

    public async Task<Answer> SaveAnswer(Answer answer)
    {
        _context.Answer.Add(answer);
        await _context.SaveChangesAsync();
        return answer;
    }

    public async Task<List<Answer>> GetAnswerByRoundId(long roundId)
    {
        return _context.Answer.Where(a => a.RoundId == roundId).ToList();
    }
    
    public async Task<Answer?> GetAnswerByRoundIdPlayerId(long roundId, string playerName)
    {
        return await _context.Answer.FirstOrDefaultAsync(a => a.RoundId == roundId && a.PlayerName == playerName);
    }
}

