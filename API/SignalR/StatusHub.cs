using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class StatusHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHubContext<PresenceHub> _presenceHub;
        private readonly PresenceTracker _presenceTracker;
        public StatusHub(IUnitOfWork unitOfWork, IMapper mapper, 
            IHubContext<PresenceHub> presenceHub,
            PresenceTracker presenceTracker)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _presenceHub = presenceHub;
            _presenceTracker = presenceTracker;
        }

        public override async Task OnConnectedAsync(){
            var httpContext = Context.GetHttpContext();

            var statuses = await _unitOfWork.StatusRepository.GetStatusesAsync();

            if(_unitOfWork.HasChanges()) await _unitOfWork.Complete();
            
            await Clients.Caller.SendAsync("StatusAll", statuses);
        }

        public override async Task OnDisconnectedAsync(Exception exception){
            await base.OnDisconnectedAsync(exception);
        }

        public async Task CreateStatus(CreateStatusDto statusDto){
            var userName = Context.User.GetUserName();
            var statusOwner = await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName);

            if(statusOwner == null) throw new HubException("Cannot Post");

            var status = new Status{
                StatusOwner = statusOwner,
                StatusOwnerName = userName,
                Content = statusDto.Content
            };

            var connections = await _presenceTracker.GetOnlineUsersIds(userName);
                if(connections != null){
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewStatusPosted", 
                            new {userName = statusOwner.UserName, knownAs = statusOwner.KnownAs});
                }

            _unitOfWork.StatusRepository.AddStatus(status);

            if(await _unitOfWork.Complete()){
                
                await Clients.Clients(connections).SendAsync("NewStatus", _mapper.Map<StatusDto>(status));
            }


        }
    }
}