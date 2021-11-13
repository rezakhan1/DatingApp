using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entity;
using API.Extension;
using API.Interface;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
     public class MessageHub : Hub
    {
        private readonly IMapper _mapper;
        private readonly IHubContext<PresenceHub> _presenceHub;
        private readonly PresenceTracker _tracker;
        private readonly IMessageRepository _ImessageRepo;
        private readonly IUserRepository _userRepo;
        public MessageHub(IMessageRepository imessaeRepository,IMapper mapper, 
        IHubContext<PresenceHub> presenceHub,
            PresenceTracker tracker,IUserRepository userRepo)
        {
            _ImessageRepo = imessaeRepository;
            _tracker = tracker;
            _presenceHub = presenceHub;
            _mapper = mapper;
            _userRepo=userRepo;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();
            var groupName = GetGroupName(Context.User.getUserName(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
           
            var group = await AddToGroup(groupName);
            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            var messages = await _ImessageRepo.
                GetMessageThread(Context.User.getUserName(), otherUser);

          //  if (_unitOfWork.HasChanges()) await _unitOfWork.Complete();

            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var group = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDTO createMessageDto)
        {
            var username = Context.User.getUserName();

            if (username == createMessageDto.RecipientUsername.ToLower())
                throw new HubException("You cannot send messages to yourself");

            var sender = await _userRepo.GetUserByUserName(username);
            var recipient = await _userRepo.GetUserByUserName(createMessageDto.RecipientUsername);

            if (recipient == null) throw new HubException("Not found user");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

         
            var groupName = GetGroupName(sender.UserName, recipient.UserName);
            var group = await _ImessageRepo.GetMessageGroup(groupName);

            if (group.Connections.Any(x => x.Username == recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            }
            else
            {
                var connections = await _tracker.GetConnectionsForUser(recipient.UserName);
                if (connections != null)
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
                        new { username = sender.UserName, knownAs = sender.KnownAs });
                }
            }

            _ImessageRepo.AddMessage(message);

            if (await _ImessageRepo.SavaAllAsync())
            {
               
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDTO>(message));
            }
        }

        private async Task<Group> AddToGroup(string groupName)
        {
            var group = await _ImessageRepo.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.getUserName());

            if (group == null)
            {
                group = new Group(groupName);
                _ImessageRepo.AddGroup(group);
            }

            group.Connections.Add(connection);

            if (await _ImessageRepo.SavaAllAsync()) return group;

            throw new HubException("Failed to join group");
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await _ImessageRepo.GetGroupForConnection(Context.ConnectionId);
            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            _ImessageRepo.RemoveConnection(connection);
            if (await _ImessageRepo.SavaAllAsync()) return group;

            throw new HubException("Failed to remove from group");
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}