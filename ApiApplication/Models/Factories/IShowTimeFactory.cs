using ApiApplication.Database.Entities;
using System;
using System.Threading.Tasks;

namespace ApiApplication.Model.Factories
{
    //Todo : To remove
    public interface IShowTimeFactory
    {
        Task<ShowtimeEntity> CreateShowtime(string movieId, int auditoriumId, DateTime sessionDate);
    }
}
