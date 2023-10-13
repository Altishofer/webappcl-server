using ToX.Models;
using ToX.Repositories;

namespace ToX.Services;

public class QuizService
{
    private readonly ApplicationContext _context;
    private readonly AnswerRepository _answerRepository;

    public QuizService(ApplicationContext context)
    {
        _context = context;
        _answerRepository = new AnswerRepository(_context);
    }
}