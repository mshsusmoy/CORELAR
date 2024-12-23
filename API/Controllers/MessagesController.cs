using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MessagesController(IUnitOfWork unitOfWork, IMapper mapper){
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto){
            var userName = User.GetUserName();
            if(userName == createMessageDto.RecepientUserName.ToLower()){
                return BadRequest("You Cannot Send Messages to Yourself");
        }
            var sender = await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName);
            var recepient = await _unitOfWork.UserRepository.GetUserByUsernameAsync(createMessageDto.RecepientUserName);

            if(recepient == null) return NotFound();

            var message = new Message{
                Sender = sender,
                Recepient = recepient,
                SendUserName = sender.UserName,
                RecepientUserName = recepient.UserName,
                Content = createMessageDto.Content
            };

            _unitOfWork.MessageRepository.AddMessage(message);

            if(await _unitOfWork.Complete()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to Send Message");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams){
             messageParams.UserName = User.GetUserName();

             var messages = await _unitOfWork.MessageRepository.GetMessagesForUser(messageParams);

             Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, 
                                                messages.TotalCount, messages.TotalPages);
             return messages;
        } 

        // [HttpGet("thread/{userName}")]
        // public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string userName){
        //     var currentUserName = User.GetUserName();

        //     return Ok(await _unitOfWork.MessageRepository.GetMessageThread(currentUserName, userName));
        // }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id){
            var userName = User.GetUserName();

            var message = await _unitOfWork.MessageRepository.GetMessage(id);

            if(message.Sender.UserName != userName && message.Recepient.UserName != userName){
                return Unauthorized();
            }

            if(message.Sender.UserName == userName) message.SenderDeleted = true;

            if(message.Recepient.UserName == userName) message.RecepientDeleted = true;

            if(message.SenderDeleted && message.RecepientDeleted) _unitOfWork.MessageRepository.DeleteMessage(message);

            if(await _unitOfWork.Complete()) return Ok();

            return BadRequest("Problem Deleting a Message");
        }
    }
}