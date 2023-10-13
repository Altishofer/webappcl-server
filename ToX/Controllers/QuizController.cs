using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToX.DTOs;
using ToX.Models;
using ToX.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ToX.DTOs;
using ToX.Models;
using ToX.Repositories;
using Host = ToX.Models.Host;

namespace ToX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public QuizController(ApplicationContext context)
        {
            _context = context;
        }
        
        [HttpGet("GetAllQuiz")]
        public async Task<IActionResult> GetAllQuiz()
        {
            List<Quiz> quizzes = _context.Quiz.ToList();
        
            return Ok(quizzes);
        }
    }
    

}