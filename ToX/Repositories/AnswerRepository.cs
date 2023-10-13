using ToX.Models;

namespace ToX.Repositories;

public class AnswerRepository
{
    private readonly ApplicationContext _context;
    
    public AnswerRepository(ApplicationContext context)
    {
        _context = context;
    }

    public List<Answer> GetAllAnswers()
    {
        return _context.Answer.ToList();
    }
}