using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entity;
using API.Extension;
using API.Helper;
using API.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
     [Authorize]
    public class MessageController : BaseController
    {
       
       private readonly IMessageRepository _messageRepository;
       private readonly IUserRepository _userRepository;
       private readonly IMapper _mapper;
        public MessageController(IUserRepository userRepository,IMessageRepository messageRepository,
                                 IMapper mapper)
        {
            _messageRepository = messageRepository;
            _userRepository=userRepository;
            _mapper=mapper;
        }

       [HttpPost]
        public async Task<ActionResult<MessageDTO>> createMessageAsync(CreateMessageDTO messageDTO )
        {
          var username=User.getUserName();
          if(username == messageDTO.RecipientUsername.ToLower()) return BadRequest("Sorry you can not send message to yourself");
          var sender=await _userRepository.GetUserByUserName(username);
          var recipient=await _userRepository.GetUserByUserName(messageDTO.RecipientUsername);
          if(recipient ==null) return  NotFound();

          var message=new Message{
              Sender=sender,
              Recipient=recipient,
              SenderUsername=sender.UserName,
              RecipientUsername=recipient.UserName,
              Content=messageDTO.Content

          };

          _messageRepository.AddMessage(message);

          if(await _messageRepository.SavaAllAsync()) return Ok(_mapper.Map<MessageDTO>(message));
          return BadRequest("Filed to send Message");

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessagesForUser([FromQuery]
            MessageParams messageParams)
        {
            messageParams.Username = User.getUserName();

            var messages = await _messageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize,
                messages.TotalCount, messages.TotalPages);

            return messages;
        }

     [HttpGet("thread/{username}")]
     public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessageThreadAsync(string userName)
     {
       var Currentusername=User.getUserName();
       return Ok(await _messageRepository.GetMessageThread(Currentusername,userName));
     }

     [HttpDelete("{Id}")]
     public async Task<ActionResult> deleteMessageAsync(int Id){
      var username = User.getUserName();

            var message = await _messageRepository.GetMessage(Id);

            if (message.Sender.UserName != username && message.Recipient.UserName != username)
                return Unauthorized();

            if (message.Sender.UserName == username) message.SenderDeleted = true;

            if (message.Recipient.UserName == username) message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
                _messageRepository.DeleteMessage(message);

            if (await _messageRepository.SavaAllAsync()) return Ok();

            return BadRequest("Problem deleting the message");
     }
    }
}