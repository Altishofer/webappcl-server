using ToX.Repositories;
using ToX.Services;

namespace ToX.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public class ChatHub : Hub
    {
        private readonly PlayerService _playerService;
        private readonly QuizService _quizService;
        private readonly RoundService _roundService;


        private Dictionary<string, List<string>> _groups = new Dictionary<string, List<string>>();

        public ChatHub(PlayerService playerService, QuizService quizService, RoundService roundService)
        {
            _playerService = playerService;
            _quizService = quizService;
            _roundService = roundService;
        }

        public async Task JoinGroup(string groupName, string playerName)
        {
            
            if (_groups.ContainsKey(groupName) && !_groups[groupName].Contains(playerName))
            {
                _groups[groupName].Add(playerName);
            }
            else if (!_groups.ContainsKey(groupName))
            {
                _groups[groupName] = new List<string>() { playerName };
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            Console.WriteLine("group: " + groupName + " connectionId: " + string.Join(" ", _groups[groupName]));
            await Clients.Group(groupName).SendAsync("ReceivePlayers", string.Join(" ", _groups[groupName]));
        }

        public async Task SendMessageToGroup(string groupName, string message)
        {
            Console.WriteLine("group: " + groupName + " message: " + message);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }

        public async Task SendNextRoundToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveRound", message);
        }

        public async Task SendPlayersToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceivePlayers", message);
        }
    }
}
