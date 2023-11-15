using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ToX.DTOs.PlayerDto;
using ToX.Hubs;
using ToX.Models;
using ToX.Repositories;
using ToX.Services;

namespace ToX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly PlayerService _playerService;
        private readonly QuizHub _quizHub;
        private readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;
        private readonly String _tokenSecret;
        private static readonly TimeSpan tokenLifetime = TimeSpan.FromHours(24);

        public PlayerController(ApplicationContext context, IConfiguration config, IHubContext<QuizHub> hubContext)
        {
            _context = context;
            _configuration = config;
            _tokenSecret = _configuration["JWT_SETTINGS_KEY"];
            _playerService = new PlayerService(_context, _configuration);
            _quizHub = new QuizHub(hubContext);
        }
        
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterPlayerDto registerPlayerDto)
        {
            if (!ModelState.IsValid){return BadRequest("Request object invalid");}
            
            if (await _playerService.PlayerExists(registerPlayerDto))
            {
                return BadRequest($"Name '{registerPlayerDto.PlayerName}' is already taken, please choose another one");
            }

            Player player = await _playerService.CreatePlayer(registerPlayerDto);
            List<Player> playerList = await _playerService.GetPlayersByQuiz(registerPlayerDto.QuizId);
            List<string> playerNames = playerList.Select(p => p.PlayerName).ToList();
            string message = string.Join(" ", playerNames);
            await _quizHub.SendPlayersToGroup(registerPlayerDto.QuizId.ToString(), message);

            return Ok(_playerService.GenerateToken(registerPlayerDto));
        }
    }
}