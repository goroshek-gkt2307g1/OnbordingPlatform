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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OnbordingPlatform.ViewModels
{
    public class AdminMainViewModel : INotifyPropertyChanged
    {
        //commands
        public ICommand ShowUsersCommand { get; } //показывает экран Пользователи
        public ICommand ShowCoursesCommand { get; } //показывает экран Курсы
        public ICommand LogoutCommand { get; } //Выход
        public ICommand ProfileCommand { get; } //Открывает Профиль
        public ICommand AddUserCommand { get; } //Добавление пользователя
        public ICommand DeleteUserCommand { get; } //Удаление пользователя
        public ICommand EditUserCommand { get; } //Редактирование пользователя
        public ICommand ConfirmAddUserCommand { get; } //Подтверждение добавления пользователя
        public ICommand CancelAddUserCommand { get; } //Отмена добавления пользователя 
        public ICommand SaveEditCommand { get; } //Сохранение редактирования пользователя
        public ICommand CancelEditCommand { get; } //Отмена редактирования пользователя
        public ICommand AddCourseCommand { get; } //Добавление курса
        public ICommand ConfirmAddCourseCommand { get; } //Подтверждение добавления курса
        public ICommand CancelAddCourseCommand { get; } //Отмена добавления курса
        public ICommand ArchivedCourseCommand { get; } //Закрытие курса
        public ICommand ConfirmArchivedCourseCommand { get; } //Подтверждение закрытия курса
        public ICommand CancelArhivedCourseCommand { get; } //Отмена закрытия курса
        public ICommand AddTaskCommand { get; } //Добавление задания
        public ICommand CancelAddTaskCommand { get; } //Отмена добавления задания
        public ICommand ConfirmAddTaskCommand { get; } //Подтверждение добавления задания
        public ICommand AddStudentCommand { get; } //Добавление ученика в курс
        public ICommand ConfirnAddStudentCommand { get; } //Подтверждение добавления ученика в курс
        public ICommand CancelAddStudentCommand { get; } //Отмена добавления ученика в курс
        public ICommand OpenSelectStudentsModalCommand { get; } //открытие модалки для выбора учеников
        public ICommand CancelSelectStudentsCommand { get; } //Отмена модалки для выбора учеников
        public ICommand ConfirmSelectStudentsCommand { get; } //Подтверждение модалки для выбора учеников
        public ICommand RemoveStudentCommand { get; } //удаление ученика из списка


        /*todo: 
        1. добавить проверку заданий по конкретному ученику
        2. добавить проверку выполнения конкретного задания по списку учеников
        3. добавить принятие/отклонение/отзыв оценки задания
        */

        //visibility 
        private Visibility _showUsersVisibility = Visibility.Visible;
        private Visibility _showCoursesVisibility = Visibility.Collapsed;
        private Visibility _showAddUserModalVisibility = Visibility.Collapsed;
        private Visibility _showEditUserModalVisibility = Visibility.Collapsed;
        private Visibility _showAddCourseModalVisibility = Visibility.Collapsed;
        private Visibility _showSelectStudentsModalVisibility = Visibility.Collapsed;

        public Visibility ShowUsersVisibility
        {
            get => _showUsersVisibility;
            set { _showUsersVisibility = value; OnPropertyChanged(); }
        }
        public Visibility ShowCoursesVisibility
        {
            get => _showCoursesVisibility;
            set { _showCoursesVisibility = value; OnPropertyChanged(); }
        }
        public Visibility ShowAddUserModalVisibility
        {
            get => _showAddUserModalVisibility;
            set { _showAddUserModalVisibility = value; OnPropertyChanged(); }
        }
        public Visibility ShowAddCourseModalVisibility
        {
            get => _showAddCourseModalVisibility;
            set { _showAddCourseModalVisibility = value; OnPropertyChanged(); }
        }
        public Visibility ShowEditUserModalVisibility
        {
            get => _showEditUserModalVisibility;
            set { _showEditUserModalVisibility = value; OnPropertyChanged(); }
        }
        public Visibility ShowSelectStudentsModalVisibility
        {
            get => _showSelectStudentsModalVisibility;
            set { _showSelectStudentsModalVisibility = value; OnPropertyChanged(); }
        }


        private Account _newUser = new Account();
        public Account NewUser
        {
            get => _newUser;
            set { _newUser = value; OnPropertyChanged(); }
        }

        private Course _newCourse = new Course();
        public Course NewCourse
        {
            get => _newCourse;
            set { _newCourse = value; OnPropertyChanged(); }
        }

        private Account _selectedUser = new Account();
        public Account SelectedUser
        {
            get => _selectedUser;
            set { _selectedUser = value; OnPropertyChanged(); }
        }

        private Course _selectedCourse = new Course();
        public Course SelectedCourse
        {
            get => _selectedCourse;
            set { _selectedCourse = value; OnPropertyChanged(); }
        }

        private bool _isEditMode = false;
        public bool IsEditMode
        {
            get => _isEditMode;
            set { _isEditMode = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Role> AvailableRoles { get; set; }
        public ObservableCollection<AccountStatus> AvailableStatuses { get; set; }
        public ObservableCollection<CourseStatus> AvailableCourseStatus { get; set; }
        public ObservableCollection<Account> AvailableAccounts { get; set; }
        public ObservableCollection<Entities.Task> AvailableTasks { get; set; }

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

        public ObservableCollection<Course> _courses;
        public ObservableCollection<Course> Courses
        {
            get => _courses;
            set { _courses = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Account> _selectedStudents;
        public ObservableCollection<Account> SelectedStudents
        {
            get => _selectedStudents;
            set { _selectedStudents = value; OnPropertyChanged(); }
        }
        private ListBox _studentsListBox;
        public ListBox StudentsListBox
        {
            get => _studentsListBox;
            set { _studentsListBox = value; OnPropertyChanged(); }
        }
        private List<int> _tempSelectedStudentIds = new List<int>();
        public string SelectedStudentsCountText => $"Выбрано учеников: {SelectedStudents.Count}";


        private string _usersCountText = "0";
        public string UsersCountText
        {
            get => _usersCountText;
            set { _usersCountText = value; OnPropertyChanged(); }
        }

        private string _coursesCountText = "0";
        public string CoursesCountText
        {
            get => _coursesCountText;
            set { _coursesCountText = value; OnPropertyChanged(); }
        }

        public AdminMainViewModel()
        {
            ShowUsersCommand = new MyCommand(ShowUsers);
            ShowCoursesCommand = new MyCommand(ShowCourses);
            LogoutCommand = new MyCommand(Logout);
            ProfileCommand = new MyCommand(Profile);
            AddUserCommand = new MyCommand(AddUser);
            DeleteUserCommand = new MyCommand<Account>(DeleteUser);
            EditUserCommand = new MyCommand<Account>(EditUser);
            ConfirmAddUserCommand = new MyCommand(ConfirmAddUser);
            CancelAddUserCommand = new MyCommand(CancelAddUser);
            SaveEditCommand = new MyCommand(SaveEdit);
            CancelEditCommand = new MyCommand(CancelEdit);
            AddCourseCommand = new MyCommand(AddCourse);
            ConfirmAddCourseCommand = new MyCommand(ConfirmAddCourse);
            CancelAddCourseCommand = new MyCommand(CancelAddCourse);
            OpenSelectStudentsModalCommand = new MyCommand(OpenSelectStudentsModal);
            CancelSelectStudentsCommand = new MyCommand(CancelSelectStudents);
            ConfirmSelectStudentsCommand = new MyCommand(ConfirmSelectStudents);
            RemoveStudentCommand = new MyCommand<Account>(RemoveStudent);
            SelectedStudents = new ObservableCollection<Account>();


            LoadUsersFromDatabase();
            LoadCoursesFromDatabase();
            LoadAvailableAccounts();
            LoadRolesAndStatuses();
        }

        private void RemoveStudent(Account student)
        {
            if (student == null) return;

            SelectedStudents.Remove(student);
            OnPropertyChanged(nameof(SelectedStudentsCountText));
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
                    NewUser = new Account();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении пользователя: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
        private void AddCourse()
        {
            NewCourse = new Course();
            var activeStatus = AvailableCourseStatus?.FirstOrDefault(s => s.StatusName == "Publish");
            if (activeStatus != null)
            {
                NewCourse.CourseStatusIdFk = activeStatus.CourseStatusId;
            }
            ShowAddCourseModalVisibility = Visibility.Visible;
        }

        private void CancelAddCourse()
        {
            ShowAddCourseModalVisibility = Visibility.Collapsed;
            NewCourse = new Course();
        }

        private void ConfirmAddCourse()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_newCourse.CourseTitle))
                {
                    MessageBox.Show("Введите название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(_newCourse.CourseDescription))
                {
                    MessageBox.Show("Введите описание", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (NewCourse.CourseStatusIdFk == 0)
                {
                    var activeStatus = AvailableCourseStatus?.FirstOrDefault(s => s.StatusName == "Publish");
                    if (activeStatus != null)
                    {
                        NewCourse.CourseStatusIdFk = activeStatus.CourseStatusId;
                    }
                }


                using (var context = new VlasovaAaКурсовая1Context())
                {
                    var courseToAdd = new Course
                    {
                        CourseTitle = NewCourse.CourseTitle,
                        CourseDescription = NewCourse.CourseDescription,
                        CourseStatusIdFk = NewCourse.CourseStatusIdFk,
                        CreationDate = new DateTime()
                    };

                    context.Courses.Add(courseToAdd);
                    context.SaveChanges();

                    var courseWithNav = context.Courses
                        .Include(c => c.CourseStatusIdFkNavigation)
                        .FirstOrDefault(c => c.CourseId == courseToAdd.CourseId);

                    if (courseWithNav != null)
                    {
                        Courses.Add(courseWithNav);
                        CoursesCountText = Courses.Count.ToString();
                    }

                    MessageBox.Show($"Курс {NewCourse.CourseTitle} успешно добавлен", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    ShowAddCourseModalVisibility = Visibility.Collapsed;
                    NewCourse = new Course();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении курса: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenSelectStudentsModal()
        {
            try
            {
                LoadAvailableAccounts();

                if (SelectedStudents == null)
                {
                    SelectedStudents = new ObservableCollection<Account>();
                }

                _tempSelectedStudentIds = SelectedStudents
                    .Where(s => s != null)
                    .Select(s => s.AccountId)
                    .ToList();

                OnPropertyChanged(nameof(AvailableAccounts));
                ShowSelectStudentsModalVisibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии выбора учеников: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelSelectStudents()
        {
            ShowSelectStudentsModalVisibility = Visibility.Collapsed;
            _tempSelectedStudentIds.Clear();
        }

        private void ConfirmSelectStudents()
        {
            try
            {
                if (StudentsListBox != null)
                {
                    var selectedAccounts = StudentsListBox.SelectedItems.Cast<Account>().ToList();

                    SelectedStudents.Clear();
                    foreach (var account in selectedAccounts)
                    {
                        SelectedStudents.Add(account);
                    }

                    OnPropertyChanged(nameof(SelectedStudentsCountText));
                }

                ShowSelectStudentsModalVisibility = Visibility.Collapsed;
                _tempSelectedStudentIds.Clear();

                MessageBox.Show($"Выбрано {SelectedStudents.Count} учеников", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении выбора: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось удалить пользователя: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void EditUser(Account user)
        {
            if (user == null) return;

            SelectedUser = new Account
            {
                AccountId = user.AccountId,
                Username = user.Username,
                Password = user.Password,
                FullName = user.FullName,
                RoleIdFk = user.RoleIdFk,
                AccountStatusFk = user.AccountStatusFk,
                AccountDescription = user.AccountDescription,
                HireDate = user.HireDate
            };

            IsEditMode = true;
            ShowEditUserModalVisibility = Visibility.Visible;
        }
        private void SaveEdit()
        {
            try
            {
                if (SelectedUser == null) return;

                if (string.IsNullOrWhiteSpace(SelectedUser.Username))
                {
                    MessageBox.Show("Введите логин", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(SelectedUser.Password))
                {
                    MessageBox.Show("Введите пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(SelectedUser.FullName))
                {
                    MessageBox.Show("Введите ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (SelectedUser.RoleIdFk == 0)
                {
                    MessageBox.Show("Выберите роль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (SelectedUser.AccountStatusFk == 0)
                {
                    MessageBox.Show("Выберите статус", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var context = new VlasovaAaКурсовая1Context())
                {
                    var userToUpdate = context.Accounts.Find(SelectedUser.AccountId);

                    if (userToUpdate != null)
                    {
                        userToUpdate.Username = SelectedUser.Username;
                        userToUpdate.Password = SelectedUser.Password;
                        userToUpdate.FullName = SelectedUser.FullName;
                        userToUpdate.RoleIdFk = SelectedUser.RoleIdFk;
                        userToUpdate.AccountStatusFk = SelectedUser.AccountStatusFk;
                        userToUpdate.AccountDescription = SelectedUser.AccountDescription;

                        context.SaveChanges();

                        var userInList = Users.FirstOrDefault(u => u.AccountId == SelectedUser.AccountId);
                        if (userInList != null)
                        {
                            var updatedUser = context.Accounts
                                .Include(a => a.RoleIdFkNavigation)
                                .Include(a => a.AccountStatusFkNavigation)
                                .FirstOrDefault(a => a.AccountId == SelectedUser.AccountId);

                            if (updatedUser != null)
                            {
                                int index = Users.IndexOf(userInList);
                                Users[index] = updatedUser;

                                Users = new ObservableCollection<Account>(Users);
                            }
                        }

                        MessageBox.Show("Данные пользователя успешно обновлены", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);

                        ShowEditUserModalVisibility = Visibility.Collapsed;
                        SelectedUser = new Account();
                        IsEditMode = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении пользователя: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CancelEdit()
        {
            ShowEditUserModalVisibility = Visibility.Collapsed;
            SelectedUser = new Account();
            IsEditMode = false;
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
        private void LoadAvailableAccounts()
        {
            try
            {
                using (var context = new VlasovaAaКурсовая1Context())
                {
                    var studentAccounts = context.Accounts
                        .Include(a => a.RoleIdFkNavigation)
                        .Where(a => a.RoleIdFkNavigation.RoleName == "Student")
                        .ToList();

                    AvailableAccounts = new ObservableCollection<Account>(studentAccounts);
                    OnPropertyChanged(nameof(AvailableAccounts));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                AvailableAccounts = new ObservableCollection<Account>();
            }
        }
        private void LoadCoursesFromDatabase()
        {
            try
            {
                using (var context = new VlasovaAaКурсовая1Context())
                {
                    var courses = context.Courses
                        .Include(a => a.MentorIdFkNavigation)
                        .Include(a => a.CourseStatusIdFkNavigation)
                        .ToList();

                    Courses = new ObservableCollection<Course>(courses);
                    CoursesCountText = Courses.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                Courses = new ObservableCollection<Course>();
                CoursesCountText = "0";
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
                    AvailableCourseStatus = new ObservableCollection<CourseStatus>(context.CourseStatuses.ToList());

                    OnPropertyChanged(nameof(AvailableRoles));
                    OnPropertyChanged(nameof(AvailableStatuses));
                    OnPropertyChanged(nameof(AvailableCourseStatus));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ролей и статусов: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}