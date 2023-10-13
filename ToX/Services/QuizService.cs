using ToX.Models;
using ToX.Repositories;

namespace ToX.Services;

public class QuizService
{
    private readonly ApplicationContext _context;
    private readonly QuizRepository _quizRepository;

    public QuizService(ApplicationContext context)
    {
        _context = context;
        _quizRepository = new QuizRepository(_context);
    }
    
    public List<Quiz> GetAllQuizzes()
    {
        return _quizRepository.GetAllQuizzes();
    }
}