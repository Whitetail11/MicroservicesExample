using MicroservicesExample.Auth.Models;

namespace MicroservicesExample.Auth.Services
{
    public interface IAuthService
    {
        Task<string> SignIn(User user);

    }
}
