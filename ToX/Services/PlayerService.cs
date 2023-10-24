using System.Collections.Generic;
using System.Threading.Tasks;
using ToX.DTOs.PlayerDto;
using ToX.Models;
using ToX.Repositories;

namespace ToX.Services;

public class PlayerService
{
    private readonly ApplicationContext _context;
    private readonly PlayerRepository _playerRepository;

    public PlayerService(ApplicationContext context)
    {
        _context = context;
        _playerRepository = new PlayerRepository(_context);
    }

    public async Task<bool> PlayerExistsByPlayerName(string playerName)
    {
        return await _playerRepository.PlayerExistsByPlayerName(playerName);
    }
    
    public async Task<Player> CreatePlayer(RegisterPlayerDto playerDto)
    {
        Player player = playerDto.toEntity();
        player.Id = await _playerRepository.NextPlayerId();
        return await _playerRepository.SavePlayer(player);
    }
    
    public async Task<Player?> GetPlayerOrNull(RegisterPlayerDto playerDto)
    {
        return await _playerRepository.GetPlayerByPlayerName(playerDto.PlayerName);
    }
    
    public async Task<List<Player>> GetAllPlayers()
    {
        return await _playerRepository.GetAllPlayers();
    }
    
    public async Task<List<Player>> GetPlayersByQuiz(long quizId)
    {
        return await _playerRepository.GetPlayersByQuiz(quizId);
    }
}