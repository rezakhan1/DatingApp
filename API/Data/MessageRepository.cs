using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entity;
using API.Extensions;
using API.Helper;
using API.Interface;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly  IMapper _mapper;
        public MessageRepository(DataContext dbContext, IMapper mapper)
        {
         _context=dbContext;
         _mapper=mapper;   
        }
        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

         public async Task<Message> GetMessage(int id)
        {
            
            return await _context.Messages
                .Include(u => u.Sender)
                .Include(u => u.Recipient)
                .SingleOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<PagedList<MessageDTO>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                .OrderByDescending(m => m.MessageSent)
               // .ProjectTo<MessageDTO>(_mapper.ConfigurationProvider)
                .AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username
                    && u.RecipientDeleted == false
                    ),
                "Outbox" => query.Where(u => u.SenderUsername == messageParams.Username
                    && u.SenderDeleted == false
                    ),
                _ => query.Where(u => u.RecipientUsername ==
                    messageParams.Username && u.RecipientDeleted == false && u.DateRead == null)
            };
            var message=query.ProjectTo<MessageDTO>(_mapper.ConfigurationProvider); 
            return await PagedList<MessageDTO>.CreateAsync(message, messageParams.PageNumber, messageParams.PageSize);

        }

        public async Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            var messages = await _context.Messages
            .Include(m=>m.Sender).ThenInclude(m=>m.Photos)
             .Include(m=>m.Recipient).ThenInclude(m=>m.Photos)
                .Where(m => m.Recipient.UserName == currentUsername
                       && m.RecipientDeleted == false
                        && m.Sender.UserName == recipientUsername
                        || m.Recipient.UserName == recipientUsername
                        && m.Sender.UserName == currentUsername && m.SenderDeleted == false
                )
                .MarkUnreadAsRead(currentUsername)
                .OrderBy(m => m.MessageSent)
               // .ProjectTo<MessageDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
            var unreadMessage=messages.Where(m=>m.DateRead==null 
                && m.Recipient.UserName==currentUsername).ToList();
            // if(unreadMessage.Any()){
            //     foreach(var message in unreadMessage){
            //         messages.DateRead=DateTime.Now;
            //     }
                await _context.SaveChangesAsync();
           // }
            return _mapper.Map<IEnumerable<MessageDTO>>(messages);
        }

        public async Task<bool> SavaAllAsync()
        {
           return await _context.SaveChangesAsync()>0;
        }
    }
}