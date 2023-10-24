using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ToX.DTOs.PlayerDto;
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
        private readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;
        private readonly String _tokenSecret;
        private static readonly TimeSpan tokenLifetime = TimeSpan.FromHours(24);

        public PlayerController(ApplicationContext context, IConfiguration config)
        {
            _context = context;
            _configuration = config;
            _tokenSecret = _configuration["JWT_SETTINGS_KEY"];
            _playerService = new PlayerService(_context, _configuration);
        }
        
        // POST: api/Player/Register
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterPlayerDto registerPlayerDto)
        {
            if (!ModelState.IsValid){return BadRequest(ModelState);}
            
            if (await _playerService.PlayerExists(registerPlayerDto))
            {
                return BadRequest($"Name '{registerPlayerDto.PlayerName}' is already taken, please choose another one");
            }

            Player player = await _playerService.CreatePlayer(registerPlayerDto);
            
            return Ok(_playerService.GenerateToken(registerPlayerDto));
        }
        
        // Get: api/Player
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPlayers()
        {
            List <Player> players = await _playerService.GetAllPlayers();
            List <ReturnPlayerDto>  returnPlayersDto = players.Select(p => new ReturnPlayerDto(p)).ToList();
            return CreatedAtAction(nameof(Register), returnPlayersDto);
        }
    }
}