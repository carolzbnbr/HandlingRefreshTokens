using HandlingRefreshTokens.Services;
using HandlingRefreshTokens.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HandlingRefreshTokens.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string username = "test";
        private string password = "test";
        public Command LoginCommand { get; }
        public IRESTService RESTService => DependencyService.Get<IRESTService>();
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }
    
        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
       

        private async void OnLoginClicked(object obj)
        {

            try
            {
                IsBusy = true;

                var response = await RESTService.AuthWithCredentialsAsync(Username, Password);

                if (response.Success)
                {
                    await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("error", "Invalid Credentials", "CANCEL");
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
            }
         
        }
    }
}
