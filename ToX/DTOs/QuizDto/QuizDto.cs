﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ToX.Models;

namespace ToX.DTOs.QuizDto;

public class QuizDto
{
    [Key]
    public long Id { get; set; }
    
    [Required]
    public long HostId { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [JsonConstructor]
    public QuizDto(){}

    public QuizDto(Quiz quiz)
    {
        Id = quiz.Id;
        HostId = quiz.HostId;
        Title = quiz.Title;
    }
    
    public QuizDto(long id, long hostId, string title)
    {
        Id = id;
        HostId = hostId;
        Title = title;
    }

    public Quiz toQuiz()
    {
        Quiz quiz = new Quiz();
        quiz.Title = Title;
        quiz.HostId = HostId;
        return quiz;
    }
}