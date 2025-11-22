using OnbordingPlatform.Commands;
using OnbordingPlatform.Domain;
using OnbordingPlatform.Entities;
using OnbordingPlatform.Models;
using OnbordingPlatform.Views;
using System.ComponentModel;
using System.Windows;

namespace OnbordingPlatform.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username = "";
        private string _password = "";
        private string _errorMessage = "";
        private readonly Auth _auth;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public MyCommand LoginCommand { get; }

        public LoginViewModel()
        {
            _auth = new Auth();
            LoginCommand = new MyCommand(LoginAsync, CanLogin);
        }

        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password);
        }

        private async void LoginAsync()
        {
            ErrorMessage = "Проверка...";
            await System.Threading.Tasks.Task.Delay(1);

            try
            {
                var account = await _auth.AuthAsync(Username, Password);

                if (account != null)
                {
                    CurrentUser.Username = account.Username;
                    CurrentUser.Role = account.RoleIdFkNavigation.RoleName;
                    OpenRoleWindow(account);
                    Application.Current.Windows.OfType<LoginWindow>().First().Close();
                }
                else
                {
                    ErrorMessage = "Неверный логин или пароль!";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка: {ex.Message}";
            }
        }

        private void OpenRoleWindow(Account account)
        {
            switch (account.RoleIdFkNavigation.RoleName)
            {
                case "Admin":
                    new Views.AdminMainWindow().Show();
                    break;
                case "Mentor":
                    new Views.MentorMainWindow().Show();
                    break;
                case "Student":
                    new Views.StudentMainWindow().Show();
                    break;
            }
            Application.Current.Windows[0].Close();

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}