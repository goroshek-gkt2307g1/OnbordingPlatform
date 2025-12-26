using Microsoft.EntityFrameworkCore;
using OnbordingPlatform.Commands;
using OnbordingPlatform.Entities;
using OnbordingPlatform.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Task = OnbordingPlatform.Entities.Task;

namespace OnbordingPlatform.ViewModels
{
	public class MentorMainViewModel : INotifyPropertyChanged
	{
		//КОМАНДЫ
		public ICommand ShowCoursesCommand { get; } //показать экран Курсы
		public ICommand LogoutCommand { get; } //выход из системы
		public ICommand AddCourseCommand { get; } //добавить курс
		public ICommand ConfirmAddCourseCommand { get; } //подтвердить добавление курса
		public ICommand CancelAddCourseCommand { get; } //отменить добавление курса
		public ICommand ArchiveCourseCommand { get; } //архивировать курс
		public ICommand ConfirmArchiveCourseCommand { get; } //подтвердить архивацию курса
		public ICommand CancelArchiveCourseCommand { get; } //отменить архивацию курса
		public ICommand AddTaskToCourseCommand { get; } //добавить задание в курс
		public ICommand CancelAddTaskCommand { get; } //отменить добавление задания
		public ICommand ConfirmAddTaskCommand { get; } //подтвердить добавление задания
		public ICommand OpenSelectStudentsModalCommand { get; } //открыть модалку выбора учеников
		public ICommand CancelSelectStudentsCommand { get; } //отменить выбор учеников
		public ICommand ConfirmSelectStudentsCommand { get; } //подтвердить выбор учеников
		public ICommand RemoveStudentCommand { get; } //удалить ученика из списка
		public ICommand OpenCourseDetailsCommand { get; } //открыть детали курса
		public ICommand OpenStudentProfileCommand { get; } //открыть профиль ученика
		public ICommand AcceptTaskCommand { get; } //принять задание
		public ICommand RejectTaskCommand { get; } //отклонить задание
		public ICommand RevokeTaskCommand { get; } //отозвать решение
		public ICommand BackToCoursesCommand { get; } //вернуться к курсам
		public ICommand CloseStudentProfileCommand { get; } //закрыть профиль студента

		//VISIBILITY
		private Visibility _showCoursesVisibility = Visibility.Visible;
		private Visibility _showAddCourseModalVisibility = Visibility.Collapsed;
		private Visibility _showSelectStudentsModalVisibility = Visibility.Collapsed;
		private Visibility _showArchiveConfirmationModalVisibility = Visibility.Collapsed;
		private Visibility _showCourseDetailsVisibility = Visibility.Collapsed;
		private Visibility _showAddTaskModalVisibility = Visibility.Collapsed;
		private Visibility _showStudentProfileModalVisibility = Visibility.Collapsed;

		public Visibility ShowCoursesVisibility //видимость экрана курсов
		{
			get => _showCoursesVisibility;
			set { _showCoursesVisibility = value; OnPropertyChanged(); }
		}

		public Visibility ShowAddCourseModalVisibility //видимость модалки добавления курса
		{
			get => _showAddCourseModalVisibility;
			set { _showAddCourseModalVisibility = value; OnPropertyChanged(); }
		}

		public Visibility ShowSelectStudentsModalVisibility //видимость модалки выбора учеников
		{
			get => _showSelectStudentsModalVisibility;
			set { _showSelectStudentsModalVisibility = value; OnPropertyChanged(); }
		}

		public Visibility ShowArchiveConfirmationModalVisibility //видимость модалки подтверждения архивации
		{
			get => _showArchiveConfirmationModalVisibility;
			set { _showArchiveConfirmationModalVisibility = value; OnPropertyChanged(); }
		}

		public Visibility ShowCourseDetailsVisibility //видимость детальки курса
		{
			get => _showCourseDetailsVisibility;
			set { _showCourseDetailsVisibility = value; OnPropertyChanged(); }
		}

		public Visibility ShowAddTaskModalVisibility //видимость модалки добавления задания
		{
			get => _showAddTaskModalVisibility;
			set { _showAddTaskModalVisibility = value; OnPropertyChanged(); }
		}

		public Visibility ShowStudentProfileModalVisibility //видимость профиля студента
		{
			get => _showStudentProfileModalVisibility;
			set { _showStudentProfileModalVisibility = value; OnPropertyChanged(); }
		}

		//BRUSH
		private Brush _coursesButtonColor = Brushes.LightGreen;
		public Brush CoursesButtonColor //цвет кнопки курсов
		{
			get => _coursesButtonColor;
			set { _coursesButtonColor = value; OnPropertyChanged(); }
		}

		//OBSERVABLE COLLECTIONS
		public ObservableCollection<CourseStatus> AvailableCourseStatus { get; set; } //доступные статусы курсов
		public ObservableCollection<Account> AvailableAccounts { get; set; } //доступные аккаунты
		private ObservableCollection<Course> _courses;
		public ObservableCollection<Course> Courses //список курсов
		{
			get => _courses;
			set { _courses = value; OnPropertyChanged(); }
		}
		private ObservableCollection<Account> _selectedStudents;
		public ObservableCollection<Account> SelectedStudents //выбранные студенты
		{
			get => _selectedStudents;
			set { _selectedStudents = value; OnPropertyChanged(); }
		}
		private ObservableCollection<StudentTaskInfo> _studentTasksInfo;
		public ObservableCollection<StudentTaskInfo> StudentTasksInfo //информация о заданиях студентов
		{
			get => _studentTasksInfo;
			set { _studentTasksInfo = value; OnPropertyChanged(); }
		}
		private ObservableCollection<Task> _courseTasks;
		public ObservableCollection<Task> CourseTasks //задания курса
		{
			get => _courseTasks;
			set { _courseTasks = value; OnPropertyChanged(); }
		}
		private ObservableCollection<Account> _courseStudents;
		public ObservableCollection<Account> CourseStudents //студенты курса
		{
			get => _courseStudents;
			set { _courseStudents = value; OnPropertyChanged(); }
		}
		private ObservableCollection<Course> _archivedCourses;
		public ObservableCollection<Course> ArchivedCourses //архивные курсы
		{
			get => _archivedCourses;
			set { _archivedCourses = value; OnPropertyChanged(); }
		}
		private ObservableCollection<Course> _publishedCourses;
		public ObservableCollection<Course> PublishedCourses //опубликованные курсы
		{
			get => _publishedCourses;
			set { _publishedCourses = value; OnPropertyChanged(); }
		}
		private ObservableCollection<SelectableAccount> _tempSelectedStudents;
		public ObservableCollection<SelectableAccount> TempSelectedStudents //временный список выбранных студентов
		{
			get => _tempSelectedStudents;
			set { _tempSelectedStudents = value; OnPropertyChanged(); }
		}

		//ТЕКСТОВЫЕ СЧЕТЧИКИ
		public string SelectedStudentsCountText => $"Выбрано учеников: {SelectedStudents.Count}"; //текст количества выбранных учеников
		private string _coursesCountText = "0";
		public string CoursesCountText //текст количества курсов
		{
			get => _coursesCountText;
			set { _coursesCountText = value; OnPropertyChanged(); }
		}

		//МОДЕЛИ
		private Course _newCourse = new Course();
		public Course NewCourse //новый курс
		{
			get => _newCourse;
			set { _newCourse = value; OnPropertyChanged(); }
		}
		private Course _selectedCourse = new Course();
		public Course SelectedCourse //выбранный курс
		{
			get => _selectedCourse;
			set { _selectedCourse = value; OnPropertyChanged(); }
		}
		private Course _courseToArchive;
		public Course CourseToArchive //курс для архивации
		{
			get => _courseToArchive;
			set { _courseToArchive = value; OnPropertyChanged(); }
		}
		private Course _currentCourseDetails;
		public Course CurrentCourseDetails //текущие детали курса
		{
			get => _currentCourseDetails;
			set { _currentCourseDetails = value; OnPropertyChanged(); }
		}
		private Account _selectedStudentProfile;
		public Account SelectedStudentProfile //выбранный профиль студента
		{
			get => _selectedStudentProfile;
			set { _selectedStudentProfile = value; OnPropertyChanged(); }
		}
		private Task _newTask = new Task();
		public Task NewTask //новое задание
		{
			get => _newTask;
			set
			{
				_newTask = value;
				if (_newTask != null && _newTask.DueDate != DateTime.MinValue)
				{
					SelectedDueDate = _newTask.DueDate; //синхронизация с выбранной датой
				}
				OnPropertyChanged();
			}
		}
		private DateTime _selectedDueDate = DateTime.Now;
		public DateTime SelectedDueDate //выбранная дата сдачи
		{
			get => _selectedDueDate;
			set
			{
				_selectedDueDate = value;
				if (_newTask != null)
				{
					_newTask.DueDate = value; //синхронизация с заданием
				}
				OnPropertyChanged();
				OnPropertyChanged(nameof(IsValidDueDate)); //уведомление об изменении валидности
			}
		}
		public bool IsValidDueDate => SelectedDueDate >= DateTime.Now.Date; //проверка что дата не меньше текущей

		//ТЕКУЩИЙ МЕНТОР
		private Account _currentMentor;
		public Account CurrentMentor //текущий ментор
		{
			get => _currentMentor;
			set { _currentMentor = value; OnPropertyChanged(); }
		}

		//КОНСТРУКТОРЫ
		public MentorMainViewModel()
		{
			//инициализация команд
			ShowCoursesCommand = new MyCommand(ShowCourses);
			LogoutCommand = new MyCommand(Logout);
			AddCourseCommand = new MyCommand(AddCourse);
			ConfirmAddCourseCommand = new MyCommand(ConfirmAddCourse);
			CancelAddCourseCommand = new MyCommand(CancelAddCourse);
			ArchiveCourseCommand = new MyCommand<Course>(ArchiveCourse);
			ConfirmArchiveCourseCommand = new MyCommand(ConfirmArchiveCourse);
			CancelArchiveCourseCommand = new MyCommand(CancelArchiveCourse);
			AddTaskToCourseCommand = new MyCommand(AddTaskToCourse);
			ConfirmAddTaskCommand = new MyCommand(ConfirmAddTask);
			CancelAddTaskCommand = new MyCommand(CancelAddTask);
			OpenSelectStudentsModalCommand = new MyCommand(OpenSelectStudentsModal);
			CancelSelectStudentsCommand = new MyCommand(CancelSelectStudents);
			ConfirmSelectStudentsCommand = new MyCommand(ConfirmSelectStudents);
			RemoveStudentCommand = new MyCommand<Account>(RemoveStudent);
			OpenCourseDetailsCommand = new MyCommand<Course>(OpenCourseDetails);
			OpenStudentProfileCommand = new MyCommand<Account>(OpenStudentProfile);
			AcceptTaskCommand = new MyCommand<Answer>(AcceptTask);
			RejectTaskCommand = new MyCommand<Answer>(RejectTask);
			RevokeTaskCommand = new MyCommand<Answer>(RevokeTask);
			BackToCoursesCommand = new MyCommand(BackToCourses);
			CloseStudentProfileCommand = new MyCommand(CloseStudentProfile);

			//инициализация текущей даты как у админа
			NewTask = new Task { DueDate = DateTime.Now.Date };
			SelectedDueDate = DateTime.Now.Date;

			//инициализация коллекций
			SelectedStudents = new ObservableCollection<Account>();
			StudentTasksInfo = new ObservableCollection<StudentTaskInfo>();
			CourseTasks = new ObservableCollection<Task>();
			CourseStudents = new ObservableCollection<Account>();
			ArchivedCourses = new ObservableCollection<Course>();
			PublishedCourses = new ObservableCollection<Course>();

			//загрузка данных
			LoadCurrentMentor(); //загрузка текущего ментора
			LoadCoursesFromDatabase(); //загрузка курсов из базы данных
			LoadAvailableAccounts(); //загрузка доступных аккаунтов
			LoadCourseStatuses(); //загрузка статусов курсов
		}

		//ЗАГРУЗКА ДАННЫХ
		private void LoadCurrentMentor() //загрузка текущего ментора
		{
			try
			{
				using (var context = new VlasovaAaКурсовая1Context())
				{
					if (!string.IsNullOrEmpty(CurrentUser.Username))
					{
						CurrentMentor = context.Accounts
							.Include(a => a.RoleIdFkNavigation)
							.FirstOrDefault(a => a.Username == CurrentUser.Username);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка загрузки данных ментора: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void LoadCoursesFromDatabase() //загрузка курсов из базы данных
		{
			try
			{
				using (var context = new VlasovaAaКурсовая1Context())
				{
					var courses = context.Courses
						.Include(c => c.CourseStatusIdFkNavigation)
						.Include(c => c.MentorIdFkNavigation)
						.Where(c => c.MentorIdFk == CurrentMentor.AccountId)
						.ToList();

					foreach (var course in courses)
					{
						UpdateCourseCounters(course);
					}

					Courses = new ObservableCollection<Course>(courses);
					UpdateCourseLists();
					CoursesCountText = Courses.Count.ToString();
					OnPropertyChanged(nameof(Courses));
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка загрузки курсов: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
				Courses = new ObservableCollection<Course>();
				ArchivedCourses = new ObservableCollection<Course>();
				PublishedCourses = new ObservableCollection<Course>();
				CoursesCountText = "0";
			}
		}

		private void LoadAvailableAccounts() //загрузка доступных аккаунтов
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
				MessageBox.Show($"Ошибка загрузки учеников: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
				AvailableAccounts = new ObservableCollection<Account>();
			}
		}

		private void LoadCourseStatuses() //загрузка статусов курсов
		{
			try
			{
				using (var context = new VlasovaAaКурсовая1Context())
				{
					AvailableCourseStatus = new ObservableCollection<CourseStatus>(context.CourseStatuses.ToList());
					OnPropertyChanged(nameof(AvailableCourseStatus));
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка загрузки статусов курсов: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void ReloadCourses() //перезагрузка курсов
		{
			try
			{
				using (var context = new VlasovaAaКурсовая1Context())
				{
					var courses = context.Courses
						.Include(c => c.CourseStatusIdFkNavigation)
						.Include(c => c.MentorIdFkNavigation)
						.Where(c => c.MentorIdFk == CurrentMentor.AccountId)
						.ToList();

					foreach (var course in courses)
					{
						UpdateCourseCounters(course);
					}

					Courses = new ObservableCollection<Course>(courses);
					UpdateCourseLists();
					CoursesCountText = Courses.Count.ToString();
					OnPropertyChanged(nameof(Courses));
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при перезагрузке курсов: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		//КОМАНДЫ С КУРСАМИ
		private void ShowCourses() //показать курсы
		{
			ShowCoursesVisibility = Visibility.Visible;
			CoursesButtonColor = Brushes.LightGreen;
		}

		private void AddCourse() //добавить курс
		{
			NewCourse = new Course();
			var activeStatus = AvailableCourseStatus?.FirstOrDefault(s => s.StatusName == "Published");
			if (activeStatus != null)
			{
				NewCourse.CourseStatusIdFk = activeStatus.CourseStatusId;
			}
			ShowAddCourseModalVisibility = Visibility.Visible;
		}

		private void CancelAddCourse() //отменить добавление курса
		{
			ShowAddCourseModalVisibility = Visibility.Collapsed;
			NewCourse = new Course();
			SelectedStudents.Clear();
			OnPropertyChanged(nameof(SelectedStudentsCountText));
		}

		private void ConfirmAddCourse() //подтвердить добавление курса
		{
			try
			{
				if (string.IsNullOrWhiteSpace(_newCourse.CourseTitle))
				{
					MessageBox.Show("Введите название курса", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}
				if (string.IsNullOrWhiteSpace(_newCourse.CourseDescription))
				{
					MessageBox.Show("Введите описание курса", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}

				using (var context = new VlasovaAaКурсовая1Context())
				{
					var publishedStatus = context.CourseStatuses
						.FirstOrDefault(s => s.StatusName == "Published");
					if (publishedStatus == null)
					{
						publishedStatus = context.CourseStatuses.FirstOrDefault();
						if (publishedStatus == null)
						{
							MessageBox.Show("В базе данных нет статусов курсов",
								"Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
							return;
						}
					}

					var courseToAdd = new Course
					{
						CourseTitle = NewCourse.CourseTitle,
						CourseDescription = NewCourse.CourseDescription,
						CourseStatusIdFk = publishedStatus.CourseStatusId,
						CreationDate = DateTime.Now,
						MentorIdFk = CurrentMentor.AccountId
					};

					context.Courses.Add(courseToAdd);
					context.SaveChanges();

					if (SelectedStudents != null && SelectedStudents.Count > 0)
					{
						var activeStatus = context.EnrollmentStatuses
							.FirstOrDefault(s => s.StatusName == "Active");
						if (activeStatus == null)
						{
							activeStatus = new EnrollmentStatus { StatusName = "Active" };
							context.EnrollmentStatuses.Add(activeStatus);
							context.SaveChanges();
						}

						foreach (var student in SelectedStudents)
						{
							var existingStudent = context.Accounts.Find(student.AccountId);
							if (existingStudent != null)
							{
								var enrollment = new Enrollment
								{
									CourseIdFk = courseToAdd.CourseId,
									AccountIdFk = student.AccountId,
									EnrollmentStatusIdFk = activeStatus.EnrollmentStatusId,
									EnrollmentDate = DateTime.Now
								};
								context.Enrollments.Add(enrollment);
							}
						}
						context.SaveChanges();
					}

					var courseWithNav = context.Courses
						.Include(c => c.CourseStatusIdFkNavigation)
						.Include(c => c.MentorIdFkNavigation)
						.FirstOrDefault(c => c.CourseId == courseToAdd.CourseId);
					if (courseWithNav != null)
					{
						UpdateCourseCounters(courseWithNav);
						Courses.Add(courseWithNav);
						CoursesCountText = Courses.Count.ToString();
						PublishedCourses.Add(courseWithNav);
						OnPropertyChanged(nameof(Courses));
					}

					MessageBox.Show($"Курс '{NewCourse.CourseTitle}' успешно создан!", "Успех",
						MessageBoxButton.OK, MessageBoxImage.Information);
					ShowAddCourseModalVisibility = Visibility.Collapsed;
					NewCourse = new Course();
					SelectedStudents.Clear();
					OnPropertyChanged(nameof(SelectedStudentsCountText));
				}
			}
			catch (DbUpdateException dbEx)
			{
				string errorDetails = dbEx.InnerException?.Message ?? dbEx.Message;
				MessageBox.Show($"Ошибка при создании курса: {errorDetails}",
					"Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при создании курса: {ex.Message}\nПодробности: {ex.InnerException?.Message}",
					"Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void ArchiveCourse(Course course) //архивировать курс
		{
			if (course == null) return;
			CourseToArchive = course;
			ShowArchiveConfirmationModalVisibility = Visibility.Visible;
		}

		private void ConfirmArchiveCourse() //подтвердить архивацию курса
		{
			try
			{
				using (var context = new VlasovaAaКурсовая1Context())
				{
					var course = context.Courses.Find(CourseToArchive.CourseId);
					if (course != null)
					{
						var archiveStatus = context.CourseStatuses
							.FirstOrDefault(s => s.StatusName == "Archive");
						if (archiveStatus != null)
						{
							course.CourseStatusIdFk = archiveStatus.CourseStatusId;
							context.SaveChanges();

							if (CurrentCourseDetails != null && CurrentCourseDetails.CourseId == course.CourseId)
							{
								CurrentCourseDetails.CourseStatusIdFk = archiveStatus.CourseStatusId;
								CurrentCourseDetails.CourseStatusIdFkNavigation = archiveStatus;
								OnPropertyChanged(nameof(CurrentCourseDetails));
							}

							var courseInList = Courses.FirstOrDefault(c => c.CourseId == course.CourseId);
							if (courseInList != null)
							{
								courseInList.CourseStatusIdFk = archiveStatus.CourseStatusId;
								courseInList.CourseStatusIdFkNavigation = archiveStatus;
								UpdateCourseLists();
								OnPropertyChanged(nameof(Courses));
							}

							MessageBox.Show("Курс переведен в архив", "Успех",
								MessageBoxButton.OK, MessageBoxImage.Information);
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при архивации курса: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
				ShowArchiveConfirmationModalVisibility = Visibility.Collapsed;
				CourseToArchive = null;
			}
		}

		private void CancelArchiveCourse() //отменить архивацию курса
		{
			ShowArchiveConfirmationModalVisibility = Visibility.Collapsed;
			CourseToArchive = null;
		}

		private void OpenCourseDetails(Course course) //открыть детали курса
		{
			if (course == null) return;
			try
			{
				using (var context = new VlasovaAaКурсовая1Context())
				{
					CurrentCourseDetails = context.Courses
						.Include(c => c.CourseStatusIdFkNavigation)
						.Include(c => c.MentorIdFkNavigation)
						.FirstOrDefault(c => c.CourseId == course.CourseId);

					if (CurrentCourseDetails != null)
					{
						var tasks = context.Tasks
							.Where(t => t.CourseIdFk == course.CourseId)
							.OrderByDescending(t => t.CreatedDate)
							.ToList();
						CourseTasks = new ObservableCollection<Task>(tasks);
						OnPropertyChanged(nameof(CourseTasks));
						LoadCourseStudents(course.CourseId);
						CurrentCourseDetails.StudentsCount = CourseStudents.Count;
						CurrentCourseDetails.TasksCount = tasks.Count;
						OnPropertyChanged(nameof(CurrentCourseDetails));

						ShowCourseDetailsVisibility = Visibility.Visible;
						ShowCoursesVisibility = Visibility.Collapsed;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при открытии деталей курса: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		//КОМАНДЫ СО СТУДЕНТАМИ
		private void RemoveStudent(Account student) //удалить студента
		{
			if (student == null) return;
			SelectedStudents.Remove(student);
			OnPropertyChanged(nameof(SelectedStudentsCountText));
		}

		private void OpenSelectStudentsModal() //открыть модалку выбора студентов
		{
			try
			{
				if (!CheckEnrollmentStatusExists())
				{
					return;
				}
				LoadAvailableAccounts();
				if (SelectedStudents == null)
				{
					SelectedStudents = new ObservableCollection<Account>();
				}

				var tempStudentList = new ObservableCollection<SelectableAccount>();
				foreach (var account in AvailableAccounts)
				{
					var selectableAccount = new SelectableAccount
					{
						Account = account,
						IsSelected = SelectedStudents.Any(s => s.AccountId == account.AccountId)
					};
					tempStudentList.Add(selectableAccount);
				}
				_tempSelectedStudents = tempStudentList;
				OnPropertyChanged(nameof(TempSelectedStudents));
				ShowSelectStudentsModalVisibility = Visibility.Visible;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при открытии выбора учеников: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void CancelSelectStudents() //отменить выбор студентов
		{
			ShowSelectStudentsModalVisibility = Visibility.Collapsed;
			_tempSelectedStudents = null;
			OnPropertyChanged(nameof(TempSelectedStudents));
		}

		private void ConfirmSelectStudents() //подтвердить выбор студентов
		{
			try
			{
				if (_tempSelectedStudents == null) return;
				SelectedStudents.Clear();
				foreach (var selectableAccount in _tempSelectedStudents.Where(s => s.IsSelected))
				{
					SelectedStudents.Add(selectableAccount.Account);
				}
				OnPropertyChanged(nameof(SelectedStudentsCountText));
				ShowSelectStudentsModalVisibility = Visibility.Collapsed;
				_tempSelectedStudents = null;
				OnPropertyChanged(nameof(TempSelectedStudents));

				if (CurrentCourseDetails != null && CurrentCourseDetails.CourseId > 0)
				{
					AddStudentsToExistingCourse(CurrentCourseDetails.CourseId);
				}
				else
				{
					MessageBox.Show($"Выбрано {SelectedStudents.Count} учеников для нового курса", "Успех",
						MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при сохранении выбора учеников: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		//КОМАНДЫ ДЕТАЛЬКИ КУРСОВ
		private void OpenStudentProfile(Account student) //открыть профиль студента
		{
			if (student == null || CurrentCourseDetails == null) return;
			try
			{
				SelectedStudentProfile = student;
				LoadStudentTasksInfo(student.AccountId);
				ShowStudentProfileModalVisibility = Visibility.Visible;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при открытии профиля студента: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void BackToCourses() //вернуться к курсам
		{
			ReloadCourses();
			ShowCourseDetailsVisibility = Visibility.Collapsed;
			ShowCoursesVisibility = Visibility.Visible;
			CurrentCourseDetails = null;
			CourseTasks.Clear();
			CourseStudents.Clear();
		}

		private void CloseStudentProfile() //закрыть профиль студента
		{
			ShowStudentProfileModalVisibility = Visibility.Collapsed;
			SelectedStudentProfile = null;
			StudentTasksInfo.Clear();
		}

		//КОМАНДЫ С ЗАДАНИЯМИ
		private void AddTaskToCourse() //добавить задание в курс
		{
			if (CurrentCourseDetails == null) return;
			using (var context = new VlasovaAaКурсовая1Context())
			{
				var courseStatus = context.CourseStatuses
					.FirstOrDefault(s => s.CourseStatusId == CurrentCourseDetails.CourseStatusIdFk);
				if (courseStatus?.StatusName == "Archive")
				{
					MessageBox.Show("Нельзя добавлять задания в архивный курс", "Ошибка",
						MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}
			}
			NewTask = new Task
			{
				DueDate = DateTime.Now.Date //установка текущей даты по умолчанию
			};
			SelectedDueDate = DateTime.Now.Date; //синхронизация выбранной даты
			ShowAddTaskModalVisibility = Visibility.Visible;
		}

		private void ConfirmAddTask() //подтвердить добавление задания
		{
			try
			{
				if (string.IsNullOrWhiteSpace(NewTask.TaskTitle))
				{
					MessageBox.Show("Введите название задания", "Ошибка",
						MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}
				if (string.IsNullOrWhiteSpace(NewTask.TaskDescription))
				{
					MessageBox.Show("Введите описание задания", "Ошибка",
						MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}

				//проверяем, что дата установлена
				if (NewTask.DueDate == DateTime.MinValue)
				{
					NewTask.DueDate = DateTime.Now.Date;
					SelectedDueDate = DateTime.Now.Date;
				}

				//проверка даты сдачи
				if (!IsValidDueDate)
				{
					MessageBox.Show("Дата сдачи не может быть раньше текущей даты", "Ошибка",
						MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}

				using (var context = new VlasovaAaКурсовая1Context())
				{
					var taskToAdd = new Task
					{
						TaskTitle = NewTask.TaskTitle,
						TaskDescription = NewTask.TaskDescription,
						CourseIdFk = CurrentCourseDetails.CourseId,
						CreatedDate = DateTime.Now,
						DueDate = NewTask.DueDate
					};
					context.Tasks.Add(taskToAdd);
					context.SaveChanges();
					CourseTasks.Insert(0, taskToAdd);
					CurrentCourseDetails.TasksCount = CourseTasks.Count;
					UpdateCourseInList(CurrentCourseDetails);
					OnPropertyChanged(nameof(CurrentCourseDetails));

					MessageBox.Show("Задание успешно добавлено", "Успех",
						MessageBoxButton.OK, MessageBoxImage.Information);
					ShowAddTaskModalVisibility = Visibility.Collapsed;
					NewTask = new Task { DueDate = DateTime.Now.Date };
					SelectedDueDate = DateTime.Now.Date;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при добавлении задания: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void CancelAddTask() //отменить добавление задания
		{
			ShowAddTaskModalVisibility = Visibility.Collapsed;
			NewTask = new Task { DueDate = DateTime.Now.Date };
			SelectedDueDate = DateTime.Now.Date;
		}

		private void AcceptTask(Answer answer) //принять задание
		{
			if (answer == null) return;
			UpdateAnswerStatus(answer, "Accepted", "Задание принято");
		}

		private void RejectTask(Answer answer) //отклонить задание
		{
			if (answer == null) return;
			UpdateAnswerStatus(answer, "Rejected", "Задание отклонено");
		}

		private void RevokeTask(Answer answer) //отозвать задание
		{
			if (answer == null) return;
			UpdateAnswerStatus(answer, "Returned", "Решение отозвано, задание возвращено на доработку");
		}

		//КОМАНДА ВЫХОДА
		private void Logout() //выход из системы
		{
			CurrentUser.Username = null;
			CurrentUser.Role = null;
			var currentWindow = Application.Current.Windows.OfType<Views.MentorMainWindow>().FirstOrDefault();
			if (currentWindow != null)
			{
				currentWindow.Close();
			}
			new Views.LoginWindow().Show();
		}

		//ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ
		private bool CheckEnrollmentStatusExists() //проверить существование статусов зачисления
		{
			try
			{
				using (var context = new VlasovaAaКурсовая1Context())
				{
					var statuses = context.EnrollmentStatuses.ToList();
					return true;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при проверке статусов зачисления: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}
		}

		private void AddStudentsToExistingCourse(int courseId) //добавить студентов в существующий курс
		{
			try
			{
				using (var context = new VlasovaAaКурсовая1Context())
				{
					var activeStatus = context.EnrollmentStatuses
						.FirstOrDefault(s => s.StatusName == "Active");
					if (activeStatus == null)
					{
						activeStatus = context.EnrollmentStatuses.FirstOrDefault();
						if (activeStatus == null)
						{
							activeStatus = new EnrollmentStatus
							{
								StatusName = "Active"
							};
							context.EnrollmentStatuses.Add(activeStatus);
							context.SaveChanges();
						}
					}

					int addedCount = 0;
					int existingCount = 0;
					int errorCount = 0;

					foreach (var student in SelectedStudents)
					{
						try
						{
							bool alreadyEnrolled = context.Enrollments
								.Any(e => e.CourseIdFk == courseId && e.AccountIdFk == student.AccountId);
							if (!alreadyEnrolled)
							{
								var enrollment = new Enrollment
								{
									CourseIdFk = courseId,
									AccountIdFk = student.AccountId,
									EnrollmentStatusIdFk = activeStatus.EnrollmentStatusId,
									EnrollmentDate = DateTime.Now
								};
								context.Enrollments.Add(enrollment);
								addedCount++;
							}
							else
							{
								existingCount++;
							}
						}
						catch (Exception ex)
						{
							errorCount++;
							Console.WriteLine($"Ошибка при добавлении ученика {student.AccountId}: {ex.Message}");
						}
					}

					if (addedCount > 0)
					{
						try
						{
							context.SaveChanges();
						}
						catch (DbUpdateException dbEx)
						{
							MessageBox.Show($"Ошибка сохранения", "Ошибка базы данных",
								MessageBoxButton.OK, MessageBoxImage.Error);
							return;
						}
					}

					if (CurrentCourseDetails != null && CurrentCourseDetails.CourseId == courseId)
					{
						UpdateCourseCounters(CurrentCourseDetails);
						LoadCourseStudents(courseId);
						OnPropertyChanged(nameof(CurrentCourseDetails));
					}

					string message = "";
					if (addedCount > 0) message += $"Успешно добавлено: {addedCount} учеников\n";
					if (!string.IsNullOrEmpty(message))
					{
						MessageBox.Show(message.Trim(), "Результат",
							MessageBoxButton.OK, MessageBoxImage.Information);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при добавлении учеников к курсу: {ex.Message}\nВнутренняя ошибка: {ex.InnerException?.Message}",
					"Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void LoadCourseStudents(int courseId) //загрузить студентов курса
		{
			try
			{
				using (var context = new VlasovaAaКурсовая1Context())
				{
					var students = context.Enrollments
						.Include(e => e.AccountIdFkNavigation)
							.ThenInclude(a => a.RoleIdFkNavigation)
						.Where(e => e.CourseIdFk == courseId &&
								   e.AccountIdFkNavigation.RoleIdFkNavigation.RoleName == "Student")
						.Select(e => e.AccountIdFkNavigation)
						.OrderBy(s => s.FullName)
						.ToList();

					CourseStudents = new ObservableCollection<Account>(students);
					OnPropertyChanged(nameof(CourseStudents));
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке учеников курса: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void LoadStudentTasksInfo(int studentId) //загрузить информацию о заданиях студента
		{
			try
			{
				using (var context = new VlasovaAaКурсовая1Context())
				{
					var courseTasks = context.Tasks
						.Where(t => t.CourseIdFk == CurrentCourseDetails.CourseId)
						.ToList();

					var studentAnswers = context.Answers
						.Include(a => a.TaskIdFkNavigation)
						.Include(a => a.ReviewIdFkNavigation)
						.Include(a => a.FeedbackIdFkNavigation)
						.Where(a => a.EmployeeIdFk == studentId &&
								   courseTasks.Select(t => t.TaskId).Contains(a.TaskIdFk))
						.ToList();

					StudentTasksInfo.Clear();
					foreach (var task in courseTasks)
					{
						var answer = studentAnswers.FirstOrDefault(a => a.TaskIdFk == task.TaskId);
						var taskInfo = new StudentTaskInfo
						{
							Task = task,
							Answer = answer,
							StudentId = studentId
						};

						if (answer == null)
						{
							taskInfo.Status = "NotSubmitted";
						}
						else if (answer.ReviewIdFkNavigation == null)
						{
							taskInfo.Status = "Submitted";
						}
						else
						{
							taskInfo.Status = answer.ReviewIdFkNavigation.ReviewName;
						}

						Console.WriteLine($"Task: {task.TaskTitle}, Status: {taskInfo.Status}, HasAnswer: {answer != null}");
						StudentTasksInfo.Add(taskInfo);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке заданий студента: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void UpdateAnswerStatus(Answer answer, string newStatus, string successMessage) //обновить статус ответа
		{
			try
			{
				using (var context = new VlasovaAaКурсовая1Context())
				{
					var answerToUpdate = context.Answers
						.Include(a => a.ReviewIdFkNavigation)
						.FirstOrDefault(a => a.AnswerId == answer.AnswerId);
					if (answerToUpdate != null)
					{
						int reviewId = GetReviewIdByStatus(newStatus, context);
						answerToUpdate.ReviewIdFk = reviewId;
						if (answerToUpdate.AnswerDate == null)
						{
							answerToUpdate.AnswerDate = DateTime.Now;
						}
						context.SaveChanges();
						LoadStudentTasksInfo(answerToUpdate.EmployeeIdFk);
						MessageBox.Show(successMessage, "Успех",
							MessageBoxButton.OK, MessageBoxImage.Information);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при обновлении статуса: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private int GetReviewIdByStatus(string status, VlasovaAaКурсовая1Context context) //получить ID рецензии по статусу
		{
			int reviewId = status.ToLower() switch
			{
				"accepted" or "принято" => 2,
				"rejected" or "отклонено" => 3,
				"returned" or "возвращено" => 4,
				"submitted" or "на проверке" => 1,
				_ => 4
			};

			var review = context.Reviews.Find(reviewId);
			if (review == null)
			{
				review = context.Reviews.FirstOrDefault();
			}
			return review?.ReviewId ?? 1;
		}

		private void UpdateCourseCounters(Course course) //обновить счетчики курса
		{
			try
			{
				using (var context = new VlasovaAaКурсовая1Context())
				{
					course.StudentsCount = context.Enrollments
						.Include(e => e.AccountIdFkNavigation)
							.ThenInclude(a => a.RoleIdFkNavigation)
						.Count(e => e.CourseIdFk == course.CourseId &&
								   e.AccountIdFkNavigation.RoleIdFkNavigation.RoleName == "Student");
					course.TasksCount = context.Tasks.Count(t => t.CourseIdFk == course.CourseId);
					OnPropertyChanged(nameof(course.StudentsCount));
					OnPropertyChanged(nameof(course.TasksCount));
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при обновлении счетчиков курса: {ex.Message}");
			}
		}

		private void UpdateCourseInList(Course updatedCourse) //обновить курс в списке
		{
			try
			{
				var courseInList = Courses.FirstOrDefault(c => c.CourseId == updatedCourse.CourseId);
				if (courseInList != null)
				{
					courseInList.StudentsCount = updatedCourse.StudentsCount;
					courseInList.TasksCount = updatedCourse.TasksCount;
					courseInList.CourseStatusIdFk = updatedCourse.CourseStatusIdFk;
					courseInList.CourseStatusIdFkNavigation = updatedCourse.CourseStatusIdFkNavigation;
					OnPropertyChanged(nameof(Courses));
					UpdateCourseLists();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при обновлении курса в списке: {ex.Message}");
			}
		}

		private string GetStudentsCountText(Course course) //получить текст количества студентов
		{
			if (course == null) return "0";
			using (var context = new VlasovaAaКурсовая1Context())
			{
				var count = context.Enrollments
					.Include(e => e.AccountIdFkNavigation)
						.ThenInclude(a => a.RoleIdFkNavigation)
					.Count(e => e.CourseIdFk == course.CourseId &&
							   e.AccountIdFkNavigation.RoleIdFkNavigation.RoleName == "Student");
				return count.ToString();
			}
		}

		private string GetTasksCountText(Course course) //получить текст количества заданий
		{
			if (course == null) return "0";
			using (var context = new VlasovaAaКурсовая1Context())
			{
				var count = context.Tasks.Count(t => t.CourseIdFk == course.CourseId);
				return count.ToString();
			}
		}

		private void UpdateCourseLists() //обновить списки курсов
		{
			var archived = new ObservableCollection<Course>();
			var published = new ObservableCollection<Course>();
			foreach (var course in Courses)
			{
				try
				{
					using (var context = new VlasovaAaКурсовая1Context())
					{
						var freshCourse = context.Courses
							.Include(c => c.CourseStatusIdFkNavigation)
							.Include(c => c.MentorIdFkNavigation)
							.FirstOrDefault(c => c.CourseId == course.CourseId);
						if (freshCourse != null)
						{
							UpdateCourseCounters(freshCourse);
							course.CourseStatusIdFkNavigation = freshCourse.CourseStatusIdFkNavigation;
							course.StudentsCount = freshCourse.StudentsCount;
							course.TasksCount = freshCourse.TasksCount;
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Ошибка при обновлении курса {course.CourseId}: {ex.Message}");
				}

				if (course.CourseStatusIdFkNavigation?.StatusName == "Archive")
				{
					archived.Add(course);
				}
				else if (course.CourseStatusIdFkNavigation?.StatusName == "Published")
				{
					published.Add(course);
				}
			}
			ArchivedCourses = archived;
			PublishedCourses = published;
			OnPropertyChanged(nameof(ArchivedCourses));
			OnPropertyChanged(nameof(PublishedCourses));
		}

		//INOTIFY_PROPERTY_CHANGED
		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propertyName = "") //уведомление об изменении свойства
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}