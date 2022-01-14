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
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class StatusController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StatusController(IUnitOfWork unitOfWork, IMapper mapper )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost("create-status")]
        public async Task<ActionResult<StatusDto>> CreateStatus(CreateStatusDto statusDto){
            var userName = User.GetUserName();
            var statusOwner = await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName);

            if(statusOwner == null) return NotFound();

            var status = new Status{
                StatusOwner = statusOwner,
                StatusOwnerName = userName,
                Content = statusDto.Content
            };

            _unitOfWork.StatusRepository.AddStatus(status);

            if(await _unitOfWork.Complete()) return Ok(_mapper.Map<StatusDto>(status));

            return BadRequest("Failed to Post");
        }

        [HttpGet("get-statuses")]
        public async Task<IEnumerable<StatusDto>> GetStatuses(){
            var statuses =  await _unitOfWork.StatusRepository.GetStatusesAsync();
            return statuses;
        } 
    }
} 