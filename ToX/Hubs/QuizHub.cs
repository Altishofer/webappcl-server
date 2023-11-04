
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
        private IHubContext<QuizHub> _hubContext;

        public QuizHub(IHubContext<QuizHub> context)
        {
            _hubContext = context;
        }

        public async Task JoinGroup(string groupName)
        {
            Console.WriteLine("Join Group: " + groupName);
            await _hubContext.Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessageToGroup(string groupName, string message)
        {
            Console.WriteLine($"SendMessageToGroup  -> {message}");
            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }
        
        public async Task SendNavigateToGroup(string groupName, string round)
        {
            Console.WriteLine($"SendNavigateToGroup  -> {round}");
            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveNavigate", round);
        }

        public async Task SendNextRoundToGroup(string groupName, string round)
        {
            Console.WriteLine($"SendNextRoundToGroup -> {round}");
            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveRound", round);
        }
        
        public async Task SendWaitRankingToGroup(string groupName, WaitResultDto resultDto)
        {
            Console.WriteLine($"SendWaitRankingToGroup -> {resultDto}");
            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveWaitResult", resultDto);
        }
        
        public async Task SendFullResultToGroup(string groupName, List<FullResultDto> resultDto)
        {
            Console.WriteLine($"SendFullResultToGroup -> {resultDto}");
            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveFullResult", resultDto);
        }

        public async Task SendPlayersToGroup(string groupName, string playerString)
        {
            Console.WriteLine($"SendPlayerToGroup -> {playerString}");
            await _hubContext.Clients.Group(groupName).SendAsync("ReceivePlayers", playerString);
        }
        
        public async Task LeaveGroup(string groupName)
        {
            Console.WriteLine($"LeaveGroup -> {groupName}");
            await _hubContext.Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
