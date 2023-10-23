using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ToX.DTOs.ResultDto;

namespace ToX.Hubs;

using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private readonly Dictionary<long, Dictionary<string, List<string>>> _groupChannelMappings = new Dictionary<long, Dictionary<string, List<string>>>();

    public async Task JoinGroup(long quizId, string? channel)
    {
        var groupName = quizId.ToString();
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        if (channel != null)
        {
            if (!_groupChannelMappings.ContainsKey(quizId))
            {
                _groupChannelMappings[quizId] = new Dictionary<string, List<string>>();
            }

            if (!_groupChannelMappings[quizId].ContainsKey(channel))
            {
                _groupChannelMappings[quizId][channel] = new List<string>();
            }

            _groupChannelMappings[quizId][channel].Add(Context.ConnectionId);
        }
    }

    public async Task LeaveGroup(long quizId, string channel)
    {
        var groupName = quizId.ToString();
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

        if (channel != null && _groupChannelMappings.ContainsKey(quizId) && _groupChannelMappings[quizId].ContainsKey(channel))
        {
            _groupChannelMappings[quizId][channel].Remove(Context.ConnectionId);
        }
    }
    
    public async Task SendMessageToGroup(long quizId, string channel, string message)
    {
        if (_groupChannelMappings.ContainsKey(quizId) && _groupChannelMappings[quizId].ContainsKey(channel))
        {
             var json = new JObject(
                new JProperty("quizId", quizId),
                new JProperty("channel", channel),
                new JProperty("message", message)
            );
            var connectionIds = _groupChannelMappings[quizId][channel];
            await Clients.Clients(connectionIds).SendAsync("ReceiveMessage", json.ToString());
        };
    }

    
    public async Task JoinGroup2(long quizId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, quizId.ToString());
    }

    public async Task LeaveGroup2(long quizId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, quizId.ToString());
    }

    public async Task SendMessageToGroup2(long quizId, string message)
    {
        await Clients.Group(quizId.ToString()).SendAsync("ReceiveMessage", message);
    }
    
    public async Task SendPlayersToGroup2(long quizId, String[] message)
    {
        await Clients.Group(quizId.ToString()).SendAsync("Players", message);
    }
    
    public async Task SendResultsToGroup2(long quizId, ResultDto message)
    {
        await Clients.Group(quizId.ToString()).SendAsync("Results", message);
    }
    
    public async Task SendNextRoundToGroup2(long quizId, ResultDto message)
    {
        await Clients.Group(quizId.ToString()).SendAsync("nextRound", message);
    }
    
}