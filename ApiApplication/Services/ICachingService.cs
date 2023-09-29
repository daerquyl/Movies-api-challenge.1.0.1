using ApiApplication.Domain.Models;
using System.Threading.Tasks;

namespace ApiApplication.Services
{
    public interface ICachingService
    {
        Task<T> TryGetAsync<T>(string key);
        Task SetAsync<T>(string key, T value);
    }
}
