using OnbordingPlatform.Commands;
using OnbordingPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace OnbordingPlatform.ViewModels
{
    public class AdminMainViewModel
    {
        //commands
        public ICommand ShowUsersCommand { get; }
        public ICommand ShowCoursesCommand { get; }
        public ICommand LogoutCommand { get; }

        //visibility 
        private string _showUsersVisibility = "Visible";
        private string _showCoursesVisibility = "Collapsed";

        public string ShowUsersVisibility
        {
            get => _showUsersVisibility;
            set { _showUsersVisibility = value; OnPropertyChanged(); }
        }

        public string ShowCoursesVisibility
        {
            get => _showCoursesVisibility;
            set { _showCoursesVisibility= value; OnPropertyChanged(); }
        }

        //color buttons
        private Brush _usersButtonColor = Brushes.LightBlue;
        private Brush _coursesButtonColor = Brushes.Transparent;

        public Brush UsersButtonColor
        {
            get => _usersButtonColor;
            set { _usersButtonColor = value; OnPropertyChanged(); }
        }

        public Brush CoursesButtonColor
        {
            get => _coursesButtonColor;
            set { _coursesButtonColor = value; OnPropertyChanged(); }
        }

        public AdminMainViewModel()
        {
            ShowUsersCommand = new MyCommand(ShowUsers);
            ShowCoursesCommand = new MyCommand(ShowCourses);
            LogoutCommand = new MyCommand(Logout);
        }

        private void ShowUsers()
        {
            ShowUsersVisibility = "Visible";
            ShowCoursesVisibility = "Collapsed";
            UsersButtonColor = Brushes.LightBlue;
            CoursesButtonColor = Brushes.Transparent;
        }

        private void ShowCourses()
        {
            ShowUsersVisibility = "Collapsed";
            ShowCoursesVisibility = "Visible";
            UsersButtonColor = Brushes.Transparent;
            CoursesButtonColor = Brushes.LightBlue;
        }
        private void Logout()
        {
            CurrentUser.Username = null;
            CurrentUser.Role = null;

            Application.Current.Windows.OfType<Views.AdminMainWindow>().First().Close();
            new Views.LoginWindow().Show();
        }

        //table 


        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
