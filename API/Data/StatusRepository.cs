using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class StatusRepository : IStatusRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public StatusRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddStatus(Status status)
        {
            _context.Statuses.Add(status);
        }

        public void DeleteStatus(Status status)
        {
            _context.Statuses.Remove(status);
        }

        public async Task<IEnumerable<StatusDto>> GetStatusesAsync()
        {
            return await _context.Statuses
                    .OrderByDescending(v => v.StatusPostTime)
                    .ProjectTo<StatusDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
        }
    }
} 