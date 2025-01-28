using backend.Domain.DTOs;
using backend.Domain.Filters;
using backend.Domain.Models;
using backend.Models;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface IReservationRepository : IRepository<ReservationModel>
    {
        Task<ReservationModel?> GetByConditionAsync(
     Expression<Func<ReservationModel, bool>> predicate,
     Func<IQueryable<ReservationModel>, IIncludableQueryable<ReservationModel, object>>? include = null);

        Task<List<ReservationModel>> GetReservationsByUserIdAsync(string userId);
        Task<ReservationModel?> GetReservationByIdAndUserIdAsync(Guid reservationId, string userId);
    }
}
