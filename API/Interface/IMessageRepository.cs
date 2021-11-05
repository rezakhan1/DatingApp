using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entity;
using API.Helper;

namespace API.Interface
{
    public interface IMessageRepository
    {
         void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDTO>> GetMessagesForUser(MessageParams message);
        Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUsername, string recipientUsername);

        Task<bool> SavaAllAsync();
    }
}