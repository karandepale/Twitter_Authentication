using System.Threading.Tasks;
using TwitterAuth_Backend.Model;

namespace TwitterAuth_Backend.Data
{
    public interface ITwitterAuthRepository
    {
        Task<RequestTokenResponse> GetRequestToken();
        Task<UserModelDto> GetAccessToken(string token, string oauthVerifier);
    }
}
