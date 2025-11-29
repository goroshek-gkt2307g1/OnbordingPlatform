using Microsoft.EntityFrameworkCore;
using OnbordingPlatform.Commands;
using OnbordingPlatform.Entities;
using OnbordingPlatform.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace OnbordingPlatform.ViewModels
{
    public class AdminMainViewModel : INotifyPropertyChanged
    {
        //commands
        public ICommand ShowUsersCommand { get; }
        public ICommand ShowCoursesCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand ProfileCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand ConfirmAddUserCommand { get; }
        public ICommand CancelAddUserCommand { get; }

        //visibility 
        private Visibility _showUsersVisibility = Visibility.Visible;
        private Visibility _showCoursesVisibility = Visibility.Collapsed;
        private Visibility _showAddUserModalVisibility = Visibility.Collapsed;

        public Visibility ShowUsersVisibility
        {
            get => _showUsersVisibility;
            set { _showUsersVisibility = value; OnPropertyChanged(); }
        }

        public Visibility ShowCoursesVisibility
        {
            get => _showCoursesVisibility;
            set { _showCoursesVisibility= value; OnPropertyChanged(); }
        }

        public Visibility ShowAddUserModalVisibility
        {
            get => _showAddUserModalVisibility;
            set { _showAddUserModalVisibility = value; OnPropertyChanged(); }
        }

        private Account _newUser = new Account();
        public Account NewUser
        {
            get => _newUser;
            set { _newUser = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Role> AvailableRoles { get; set; }
        public ObservableCollection<AccountStatus> AvailableStatuses { get; set; }


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

        public ObservableCollection<Account> _users;
        public ObservableCollection<Account> Users
        {
            get => _users;
            set { _users = value; OnPropertyChanged(); }
        }

        private string _usersCountText = "0";
        public string UsersCountText
        {
            get => _usersCountText;
            set { _usersCountText = value; OnPropertyChanged(); }
        }

        public AdminMainViewModel()
        {
            ShowUsersCommand = new MyCommand(ShowUsers);
            ShowCoursesCommand = new MyCommand(ShowCourses);
            LogoutCommand = new MyCommand(Logout);
            ProfileCommand = new MyCommand(Profile);
            AddUserCommand = new MyCommand(AddUser);
            DeleteUserCommand = new MyCommand<Account>(DeleteUser);
            ConfirmAddUserCommand = new MyCommand(ConfirmAddUser);
            CancelAddUserCommand = new MyCommand(CancelAddUser);

            LoadUsersFromDatabase();
            LoadRolesAndStatuses();
        }

        private void CancelAddUser()
        {
            ShowAddUserModalVisibility = Visibility.Collapsed;
            NewUser = new Account();

        }

        private void ConfirmAddUser()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_newUser.Username))
                {
                    MessageBox.Show("Введите логин", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(_newUser.Password))
                {
                    MessageBox.Show("Введите пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(_newUser.FullName))
                {
                    MessageBox.Show("Введите ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (NewUser.RoleIdFk == 0)
                {
                    MessageBox.Show("Выберите роль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (NewUser.AccountStatusFk == 0)
                {
                    var activeStatus = AvailableStatuses?.FirstOrDefault(s => s.StatusName == "Active");
                    if (activeStatus != null)
                    {
                        NewUser.AccountStatusFk = activeStatus.AccountStatusId;
                    }
                }
                NewUser.HireDate = DateOnly.FromDateTime(DateTime.Now);

                using (var context = new VlasovaAaКурсовая1Context())
                {
                    var userToAdd = new Account
                    {
                        Username = NewUser.Username,
                        Password = NewUser.Password,
                        FullName = NewUser.FullName,
                        RoleIdFk = NewUser.RoleIdFk,
                        AccountStatusFk = NewUser.AccountStatusFk,
                        AccountDescription = NewUser.AccountDescription,
                        HireDate = NewUser.HireDate
                    };

                    context.Accounts.Add(userToAdd);
                    context.SaveChanges();

                    var userWithNav = context.Accounts
                        .Include(a => a.RoleIdFkNavigation)
                        .Include(a => a.AccountStatusFkNavigation)
                        .FirstOrDefault(a => a.AccountId == userToAdd.AccountId);

                    if (userWithNav != null)
                    {
                        Users.Add(userWithNav);
                        UsersCountText = Users.Count.ToString();
                    }

                    MessageBox.Show($"Пользователь {NewUser.Username} успешно добавлен", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    ShowAddUserModalVisibility = Visibility.Collapsed;
                    NewUser = new Account(); // Очищаем форму
                }
            }
            catch (Exception ex)
            {
                // Получаем детальную информацию об ошибке
                string errorDetails = GetExceptionDetails(ex);
                MessageBox.Show($"Ошибка при добавлении пользователя:\n\n{errorDetails}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для получения детальной информации об исключении
        private string GetExceptionDetails(Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Сообщение: {ex.Message}");

            if (ex.InnerException != null)
            {
                sb.AppendLine($"Внутреннее исключение: {ex.InnerException.Message}");

                // Для DbUpdateException получаем больше деталей
                if (ex.InnerException is Microsoft.Data.SqlClient.SqlException sqlEx)
                {
                    sb.AppendLine($"Код ошибки SQL: {sqlEx.Number}");
                    sb.AppendLine($"Процедура: {sqlEx.Procedure}");
                    sb.AppendLine($"Строка: {sqlEx.LineNumber}");
                }
            }

            sb.AppendLine($"Тип исключения: {ex.GetType().Name}");
            sb.AppendLine($"Стек вызовов: {ex.StackTrace}");

            return sb.ToString();
        }

        private void ShowUsers()
        {
            ShowUsersVisibility = Visibility.Visible;
            ShowCoursesVisibility = Visibility.Collapsed;
            UsersButtonColor = Brushes.LightBlue;
            CoursesButtonColor = Brushes.Transparent;
        }

        private void ShowCourses()
        {
            ShowUsersVisibility = Visibility.Collapsed;
            ShowCoursesVisibility = Visibility.Visible;
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

        private void Profile()
        {
            MessageBox.Show("Функционал профиля", "Профиль", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddUser()
        {
            NewUser = new Account();
            var activeStatus = AvailableStatuses?.FirstOrDefault(s => s.StatusName == "Active");
            if (activeStatus != null)
            {
                NewUser.AccountStatusFk = activeStatus.AccountStatusId;
            }
            ShowAddUserModalVisibility = Visibility.Visible;
        }
        private void DeleteUser(Account userForDelete)
        {
            if (userForDelete == null) return;

            if (userForDelete.Username == CurrentUser.Username)
            {
                MessageBox.Show("Вы не можете удалить себя!", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show($"Вы действительно хотите удалить {userForDelete.Username}?", "Удалить пользователя", 
                MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            if (result == MessageBoxResult.OK)
            {
                try
                {
                    using (var context = new VlasovaAaКурсовая1Context())
                    {
                        var user = context.Accounts.Find(userForDelete.AccountId);
                        if (user != null)
                        {
                            context.Accounts.Remove(user);
                            context.SaveChanges();

                            Users.Remove(userForDelete);
                            UsersCountText = Users.Count.ToString();

                            MessageBox.Show("Пользователь успешно удален", "Успех",
                                          MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }

                catch
                {
                    MessageBox.Show("Не удалось удалить пользователя!", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else return;

        }


        private void LoadUsersFromDatabase()
        {
            try
            {
                using (var context = new VlasovaAaКурсовая1Context()) 
                {
                    var users = context.Accounts
                        .Include(a => a.RoleIdFkNavigation)
                        .Include(a => a.AccountStatusFkNavigation)
                        .ToList();

                    Users = new ObservableCollection<Account>(users);
                    UsersCountText = Users.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                Users = new ObservableCollection<Account>();
                UsersCountText = "0";
            }
        }

        private void LoadRolesAndStatuses()
        {
            try
            {
                using (var context = new VlasovaAaКурсовая1Context())
                {
                    AvailableRoles = new ObservableCollection<Role>(context.Roles.ToList());
                    AvailableStatuses = new ObservableCollection<AccountStatus>(context.AccountStatuses.ToList());

                    OnPropertyChanged(nameof(AvailableRoles));
                    OnPropertyChanged(nameof(AvailableStatuses));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ролей и статусов: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
