using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            _playerService = new PlayerService(_context);
        }
        
        // POST: api/Player/Register
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterPlayerDto registerPlayerDto)
        {
            if (!ModelState.IsValid){return BadRequest(ModelState);}
            
            if (await _context.Player.AnyAsync(u => u.PlayerName == registerPlayerDto.PlayerName))
            {
                ModelState.AddModelError("PlayerName", "PlayerName is already taken.");
                return BadRequest(ModelState);
            }
            
            Player player = registerPlayerDto.toEntity();
            player.Id = await _context.Player.AnyAsync() ? (await _context.Player.MaxAsync(u => u.Id)) + 1 : 0;

            _context.Player.Add(player);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(Register), new ReturnPlayerDto(player));
        }
        
        // Get: api/Player
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPlayers()
        {
            List <Player> players = _playerService.GetAllPlayer();
            List <ReturnPlayerDto>  returnPlayersDto = players.Select(p => new ReturnPlayerDto(p)).ToList();
            return CreatedAtAction(nameof(Register), returnPlayersDto);
        }
    }
}