using IdentityService.Api.Model;

namespace IdentityService.Api.Services
{
    public interface IIdentityService
    {
        Task<LoginResponseModel> Login(LoginRequestModel requestModel);
    }
}
