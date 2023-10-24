
using ToX.Models;
using ToX.Repositories;
using ToX.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ToX.DTOs.ResultDto;


namespace ToX.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public class QuizHub : Hub
    {
        private readonly PlayerService _playerService;
        private readonly QuizService _quizService;
        private readonly RoundService _roundService;
        
        private Dictionary<string, List<string>> _groups = new Dictionary<string, List<string>>();

        public QuizHub(PlayerService playerService, QuizService quizService, RoundService roundService)
        {
            _playerService = playerService;
            _quizService = quizService;
            _roundService = roundService;
        }

        public async Task JoinGroup(string groupName, string playerName)
        {
            Console.WriteLine("Join Group: " + groupName + " Player: " + playerName);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessageToGroup(string groupName, string message)
        {
            Console.WriteLine("group: " + groupName + " message: " + message);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }

        public async Task SendNextRoundToGroup(string groupName, string round)
        {
            await Clients.Group(groupName).SendAsync("ReceiveRound", round);
        }

        public async Task SendPlayersToGroup(string groupName)
        {
            List<Player> players = await _playerService.GetPlayersByQuiz(long.Parse(groupName));
            await Clients.Group(groupName).SendAsync("ReceivePlayers", string.Join(" ", players.Select(p => p.PlayerName)));
        }
        
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
