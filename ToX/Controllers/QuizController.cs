using System.Collections.Generic;
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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ToX.DTOs;
using ToX.DTOs.QuizDto;
using ToX.DTOs.RoundDto;
using ToX.Models;
using ToX.Repositories;
using ToX.Services;
using Host = ToX.Models.Host;

namespace ToX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IConfiguration _config;
        private readonly QuizService _quizService;
        private readonly RoundService _roundService;
        private readonly AnswerService _answerService;
        private readonly Word2VectorService _word2VectorService;

        public QuizController(ApplicationContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _word2VectorService = Word2VectorService.GetInstance(_context, _config);
            _quizService = new QuizService(_context);
            _roundService = new RoundService(_context, _word2VectorService);
            _answerService = new AnswerService(_context, _word2VectorService);
        }

        [HttpGet("GetAllQuizzes")]
        public async Task<IActionResult> GetAllQuizzes()
        {
            List<Quiz> quizzes = await _quizService.GetAllQuizzes();
            return Ok(quizzes);
        }

        [HttpGet("GetAllRounds")]
        public async Task<IActionResult> GetAllRounds()
        {
            List<Round> rounds = await _roundService.GetAllRounds();
            return Ok(rounds);
        }

        [HttpGet("GetAllAnswers")]
        public async Task<IActionResult> GetAllAnswers()
        {
            List<Answer> answers = await _answerService.GetAllAnswers();
            return Ok(answers);
        }

        [HttpPost("CreateQuiz")]
        public async Task<IActionResult> CreateQuiz([FromBody] QuizDto quizDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Quiz quiz = await _quizService.CreateQuiz(quizDto);
            QuizDto returnQuizDto = new QuizDto(quiz);
            return CreatedAtAction(nameof(CreateQuiz), new { returnQuizDto });
        }

        [HttpPost("CreateRound")]
        public async Task<IActionResult> CreateRound(RoundDto roundDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Round round = await _roundService.CreateRound(roundDto);
            RoundDto returnRoundDto = new RoundDto(round);
            return CreatedAtAction(nameof(CreateRound), new { returnRoundDto });
        }

        [HttpPost("CreateAnswer")]
        public async Task<IActionResult> CreateAnswer(AnswerDto answerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Answer answer = await _answerService.CreateAnswer(answerDto);
            AnswerDto returnAnswerDto = new AnswerDto(answer);
            return CreatedAtAction(nameof(CreateAnswer), new { returnAnswerDto });
        }

        [HttpGet("GetQuiz/{id}")]
        public async Task<IActionResult> GetAllQuiz([FromRoute] long id)
        {
            Quiz? quiz = await _quizService.GetQuizOrNull(id);
            if (quiz == null)
            {
                return NotFound("quiz not found");
            }

            QuizDto quizDto = new QuizDto(quiz);
            return Ok(new { quizDto });
        }


        [HttpGet("GetRound/{id}")]
        public async Task<IActionResult> GetRound([FromRoute] long id)
        {
            Round? round = await _roundService.GetRoundOrNull(id);
            if (round == null)
            {
                return NotFound("round not found");
            }

            RoundDto roundDto = new RoundDto(round);
            return Ok(new { roundDto });
        }

        [HttpGet("GetAnswer/{id}")]
        public async Task<IActionResult> GetAnswer([FromRoute] long id)
        {
            Answer? answer = await _answerService.GetAnswerOrNull(id);
            if (answer == null)
            {
                return NotFound("answer not found");
            }

            AnswerDto answerDto = new AnswerDto(answer);
            return Ok(new { answerDto });
        }
    }
}