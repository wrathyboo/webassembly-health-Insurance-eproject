using HealthInsurance.Shared;

namespace HealthInsurance.Client.Services
{
    public interface IAuthService
    {
      
            Task<RegisterResult> Register(RegisterModel registerModel);

            Task<LoginResult> Login(LoginModel loginModel);

            Task Logout();
        
    }
}
