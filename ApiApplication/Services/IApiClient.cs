using ProtoDefinitions;
using System.Threading.Tasks;

namespace ApiApplication.Services
{
    public interface IApiClient
    {
        Task<showListResponse> GetAll();
        Task<showResponse> GetMovie(string movieId);
    }
}