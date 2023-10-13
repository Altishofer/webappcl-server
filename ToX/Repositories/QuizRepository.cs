using ToX.Models;

namespace ToX.Repositories;

public class QuizRepository
{
    private readonly ApplicationContext _context;

    public QuizRepository(ApplicationContext context)
    {
        _context = context;
    }

    public List<Quiz> GetAllQuizzes()
    {
        return _context.Quiz.ToList();
    }
}