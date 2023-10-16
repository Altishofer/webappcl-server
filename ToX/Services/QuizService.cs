using System.Collections.Generic;
using System.Threading.Tasks;
using ToX.DTOs.QuizDto;
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
    
    public async Task<List<Quiz>> GetAllQuizzes()
    {
        return await _quizRepository.GetAllQuizzes();
    }
    
    public async Task<Quiz> CreateQuiz(QuizDto quizDto)
    {
        Quiz quiz = quizDto.toQuiz();
        quiz.Id = await _quizRepository.NextQuizId();
        return await _quizRepository.SaveQuiz(quiz);
    }
    
    public async Task<Quiz?> GetQuizOrNull(long id)
    {
        return await _quizRepository.GetQuizById(id);
    }
}