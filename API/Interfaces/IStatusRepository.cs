using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IStatusRepository
    {
        void AddStatus(Status status);
        void DeleteStatus(Status status);
        Task<IEnumerable<StatusDto>> GetStatusesAsync();
    }
} 