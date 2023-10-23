using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ToX.DTOs;
using ToX.DTOs.PlayerDto;
using ToX.Models;
using ToX.Repositories;

namespace ToX.Services;

public class PlayerService
{
    private readonly ApplicationContext _context;
    private readonly PlayerRepository _playerRepository;
    private readonly IConfiguration _configuration;
    private readonly String _tokenSecret;
    private static readonly TimeSpan tokenLifetime = TimeSpan.FromHours(24);

    public PlayerService(ApplicationContext context, IConfiguration configuration)
    {
        _context = context;
        _playerRepository = new PlayerRepository(_context);
        _configuration = configuration;
        _tokenSecret = _configuration["JWT_SETTINGS_KEY"];
    }

    public async Task<bool> PlayerExists(RegisterPlayerDto playerDto)
    {
        return await _playerRepository.PlayerExists(playerDto.PlayerName, playerDto.QuizId);
    }
    
    public async Task<Player> CreatePlayer(RegisterPlayerDto playerDto)
    {
        Player player = playerDto.toEntity();
        player.Id = await _playerRepository.NextPlayerId();
        return await _playerRepository.SavePlayer(player);
    }
    
    public async Task<Player?> GetPlayerOrNull(RegisterPlayerDto playerDto)
    {
        return await _playerRepository.GetPlayer(playerDto.PlayerName, playerDto.QuizId);
    }
    
    public async Task<List<Player>> GetAllPlayers()
    {
        return await _playerRepository.GetAllPlayers();
    }
    
    public async Task<string> GenerateToken(RegisterPlayerDto registerPlayerDto)
    {
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.UTF8.GetBytes(_tokenSecret);

        List<Claim> claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, registerPlayerDto.PlayerName),
            new("playerName", registerPlayerDto.PlayerName),
            new("quizId", registerPlayerDto.QuizId.ToString())
        };

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(tokenLifetime),
            Issuer = _configuration["JWT_SETTINGS_ISSUER"],
            Audience = _configuration["JWT_SETTINGS_AUDIENCE"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        SecurityToken token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);

        String jwt = jwtSecurityTokenHandler.WriteToken(token);
            
        return jwt;
    }
}