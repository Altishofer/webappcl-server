﻿using Microsoft.EntityFrameworkCore;
using ToX.Models;

namespace ToX.Repositories;

public class QuizRepository
{
    private readonly ApplicationContext _context;

    public QuizRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public void DeleteQuiz(Quiz quiz)
    {
        _context.Remove(quiz);
    }

    public async Task<List<Quiz>> GetAllQuizzes()
    {
        return _context.Quiz.ToList();
    }
    
    public async Task<List<Quiz>> GetQuizzesByHostId(long id)
    {
        return _context.Quiz.Where(h => h.HostId == id).ToList();
    }
    
    public async Task<Quiz?> GetQuizById(long quizId)
    {
        return await _context.Quiz.FindAsync(quizId);
    }
    
    public async Task<bool> QuizExistsById(long quizId)
    {
        return await _context.Quiz.AnyAsync(h => h.Id == quizId);
    }

    public async Task<long> NextQuizId()
    {
        return await _context.Quiz.AnyAsync() ? (await _context.Quiz.MaxAsync(u => u.Id)) + 1 : 0;
    }

    public async Task<Quiz> SaveQuiz(Quiz quiz)
    {
        _context.Quiz.Add(quiz);
        await _context.SaveChangesAsync();
        return quiz;
    }
}