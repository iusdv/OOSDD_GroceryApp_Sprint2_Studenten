using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IClientService _clientService;

        public AuthService(IClientService clientService)
        {
            _clientService = clientService;
        }
        public Client? Login(string email, string password)
        {
            // client id
            // verify password
            var client = _clientService.Get(email);
            if (client is null) return null;
            
            var isValid = PasswordHelper.VerifyPassword(password, client.Password);
            if (!isValid) return null;
            client.Password = string.Empty;
            
            return client;
            
        }
    }
}
