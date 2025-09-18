using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;

        [ObservableProperty] private string email = string.Empty;
        [ObservableProperty] private string password = string.Empty;
        [ObservableProperty] private string? loginMessage = string.Empty; 

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        private async Task Login()
        {
            LoginMessage = null;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                LoginMessage = "Vul e-mail en wachtwoord in.";
                return;
            }

            // Login Auth
            Client? client = _authService.Login(Email, Password);
            if (client is null)
            {
                LoginMessage = "Onjuist e-mailadres of wachtwoord.";
                return;
            }
            
            Application.Current.MainPage = new AppShell();
            await Task.CompletedTask;
        }
    }
}
