using Microsoft.EntityFrameworkCore;
using OnbordingPlatform.Commands;
using OnbordingPlatform.Entities;
using OnbordingPlatform.Models;
using OnbordingPlatform.Models.Entities;
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
	public class StudentMainViewModel : INotifyPropertyChanged
	{
		//КОМАНДЫ
		public ICommand ShowCoursesCommand { get; } //показать экран курсов
		public ICommand LogoutCommand { get; } //выход из системы
		public ICommand OpenCourseDetailsCommand { get; } //открыть детали курса
		public ICommand SubmitAnswerCommand { get; } //отправить ответ
		public ICommand CancelSubmitCommand { get; } //отменить отправку
		public ICommand ConfirmSubmitCommand { get; } //подтвердить отправку
		public ICommand BackToCoursesCommand { get; } //вернуться к курсам
		public ICommand RefreshCoursesCommand { get; } //обновить курсы

		//VISIBILITY 
		private Visibility _showCoursesVisibility = Visibility.Visible;
		private Visibility _showCourseDetailsVisibility = Visibility.Collapsed;
		private Visibility _showSubmitModalVisibility = Visibility.Collapsed;

		public Visibility ShowCoursesVisibility //видимость экрана курсов
		{
			get => _showCoursesVisibility;
			set { _showCoursesVisibility = value; OnPropertyChanged(); }
		}

		public Visibility ShowCourseDetailsVisibility //видимость деталей курса
		{
			get => _showCourseDetailsVisibility;
			set { _showCourseDetailsVisibility = value; OnPropertyChanged(); }
		}

		public Visibility ShowSubmitModalVisibility //видимость модалки отправки ответа
		{
			get => _showSubmitModalVisibility;
			set { _showSubmitModalVisibility = value; OnPropertyChanged(); }
		}

		//OBSERVABLE COLLECTIONS
		private ObservableCollection<Course> _studentCourses;
		public ObservableCollection<Course> StudentCourses //курсы студента
		{
			get => _studentCourses;
			set
			{
				_studentCourses = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<CourseTaskInfo> CourseTasks { get; set; } //задания курса

		//МОДЕЛИ
		private Course _currentCourseDetails;
		public Course CurrentCourseDetails //текущие детали курса
		{
			get => _currentCourseDetails;
			set { _currentCourseDetails = value; OnPropertyChanged(); }
		}

		private CourseTaskInfo _selectedTaskForAnswer;
		public CourseTaskInfo SelectedTaskForAnswer //выбранное задание для ответа
		{
			get => _selectedTaskForAnswer;
			set
			{
				_selectedTaskForAnswer = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(IsTaskSelected));
			}
		}

		//ОТВЕТЫ
		private string _answerText;
		public string AnswerText //текст ответа
		{
			get => _answerText;
			set { _answerText = value; OnPropertyChanged(); }
		}

		private string _answerDocumentPath;
		public string AnswerDocumentPath //путь к документу ответа
		{
			get => _answerDocumentPath;
			set { _answerDocumentPath = value; OnPropertyChanged(); }
		}

		//ВЫЧИСЛЯЕМЫЕ СВОЙСТВА
		public bool IsTaskSelected => SelectedTaskForAnswer != null; //выбрано ли задание

		//СТАТИСТИКА
		private int _totalTasks;
		private int _completedTasks;
		private int _pendingTasks;

		public string TotalTasksText => $"Всего заданий: {_totalTasks}"; //текст общего количества заданий
		public string CompletedTasksText => $"Выполнено: {_completedTasks}"; //текст выполненных заданий
		public string PendingTasksText => $"Ожидает проверки: {_pendingTasks}"; //текст заданий на проверке

		//BRUSH 
		private Brush _coursesButtonColor = Brushes.LightBlue;
		private Brush _profileButtonColor = Brushes.Transparent;

		public Brush CoursesButtonColor //цвет кнопки курсов
		{
			get => _coursesButtonColor;
			set { _coursesButtonColor = value; OnPropertyChanged(); }
		}

		public Brush ProfileButtonColor //цвет кнопки профиля
		{
			get => _profileButtonColor;
			set { _profileButtonColor = value; OnPropertyChanged(); }
		}

		//КОНСТРУКТОРЫ
		public StudentMainViewModel()
		{
			//инициализация команд
			ShowCoursesCommand = new MyCommand(ShowCourses);
			LogoutCommand = new MyCommand(Logout);
			OpenCourseDetailsCommand = new MyCommand<Course>(OpenCourseDetails);
			SubmitAnswerCommand = new MyCommand<CourseTaskInfo>(SubmitAnswer);
			CancelSubmitCommand = new MyCommand(CancelSubmit);
			ConfirmSubmitCommand = new MyCommand(ConfirmSubmit);
			BackToCoursesCommand = new MyCommand(BackToCourses);
			RefreshCoursesCommand = new MyCommand(RefreshCourses);

			//инициализация коллекций
			StudentCourses = new ObservableCollection<Course>();
			CourseTasks = new ObservableCollection<CourseTaskInfo>();

			//загрузка данных
			LoadStudentCourses(); //загрузка курсов студента
		}

		//ЗАГРУЗКА ДАННЫХ
		private void LoadStudentCourses() //загрузка курсов студента
		{
			try
			{
				using (var context = new VlasovaAaКурсовая1Context())
				{
					var currentUser = context.Accounts
						.FirstOrDefault(a => a.Username == CurrentUser.Username);

					if (currentUser == null) return;

					var enrollments = context.Enrollments
						.Include(e => e.CourseIdFkNavigation)
							.ThenInclude(c => c.CourseStatusIdFkNavigation)
						.Include(e => e.CourseIdFkNavigation)
							.ThenInclude(c => c.MentorIdFkNavigation)
						.Where(e => e.AccountIdFk == currentUser.AccountId &&
								   e.EnrollmentStatusIdFkNavigation.StatusName != "Cancelled")
						.ToList();

					StudentCourses.Clear();

					foreach (var enrollment in enrollments)
					{
						var course = enrollment.CourseIdFkNavigation;
						if (course != null)
						{
							//количество заданий для курса
							course.TasksCount = context.Tasks
								.Count(t => t.CourseIdFk == course.CourseId);

							//количество студентов для курса
							course.StudentsCount = context.Enrollments
								.Count(e => e.CourseIdFk == course.CourseId &&
										   e.EnrollmentStatusIdFkNavigation.StatusName != "Cancelled");

							//проверка, что навигационные свойства загружены
							if (course.CourseStatusIdFkNavigation == null)
							{
								course.CourseStatusIdFkNavigation = context.CourseStatuses
									.FirstOrDefault(s => s.CourseStatusId == course.CourseStatusIdFk);
							}

							if (course.MentorIdFkNavigation == null)
							{
								course.MentorIdFkNavigation = context.Accounts
									.FirstOrDefault(a => a.AccountId == course.MentorIdFk);
							}

							StudentCourses.Add(course);
						}
					}

					OnPropertyChanged(nameof(StudentCourses));

					LoadOverallStatistics(currentUser.AccountId);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке курсов: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
				StudentCourses = new ObservableCollection<Course>();
			}
		}

		private void LoadOverallStatistics(int studentId) //загрузка общей статистики
		{
			try
			{
				using (var context = new VlasovaAaКурсовая1Context())
				{
					//все курсы студента
					var courseIds = StudentCourses.Select(c => c.CourseId).ToList();

					if (courseIds.Count == 0)
					{
						_totalTasks = 0;
						_completedTasks = 0;
						_pendingTasks = 0;
						UpdateStatisticsUI();
						return;
					}

					//задания из всех курсов студента
					var allTasks = context.Tasks
						.Where(t => courseIds.Contains(t.CourseIdFk))
						.Select(t => t.TaskId)
						.ToList();

					_totalTasks = allTasks.Count;

					//все ответы студента по этим заданиям
					var studentAnswers = context.Answers
						.Include(a => a.ReviewIdFkNavigation)
						.Where(a => a.EmployeeIdFk == studentId &&
								   allTasks.Contains(a.TaskIdFk))
						.ToList();

					//выполненные и ожидающие проверки
					_completedTasks = studentAnswers
						.Count(a => a.ReviewIdFkNavigation != null &&
								   (a.ReviewIdFkNavigation.ReviewName == "Accepted" ||
									a.ReviewIdFkNavigation.ReviewName == "Принято"));

					_pendingTasks = studentAnswers
						.Count(a => a.ReviewIdFkNavigation != null &&
								   (a.ReviewIdFkNavigation.ReviewName == "Submitted" ||
									a.ReviewIdFkNavigation.ReviewName == "На проверке"));

					UpdateStatisticsUI();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при загрузке статистики: {ex.Message}");
				_totalTasks = 0;
				_completedTasks = 0;
				_pendingTasks = 0;
				UpdateStatisticsUI();
			}
		}

		private void UpdateStatisticsUI() //обновление UI статистики
		{
			OnPropertyChanged(nameof(TotalTasksText));
			OnPropertyChanged(nameof(CompletedTasksText));
			OnPropertyChanged(nameof(PendingTasksText));
		}

		private void LoadCourseTasksWithStatus(int courseId) //загрузка заданий курса со статусами
		{
			try
			{
				CourseTasks.Clear();

				using (var context = new VlasovaAaКурсовая1Context())
				{
					var tasks = context.Tasks
						.Where(t => t.CourseIdFk == courseId)
						.OrderBy(t => t.CreatedDate)
						.ToList();

					var currentUser = context.Accounts
						.FirstOrDefault(a => a.Username == CurrentUser.Username);

					if (currentUser == null) return;

					var allReviews = context.Reviews.ToDictionary(r => r.ReviewId, r => r);

					foreach (var task in tasks)
					{
						try
						{
							var answer = context.Answers
								.AsNoTracking()
								.FirstOrDefault(a => a.TaskIdFk == task.TaskId &&
													a.EmployeeIdFk == currentUser.AccountId);

							var taskInfo = new CourseTaskInfo
							{
								Task = task,
								StudentId = currentUser.AccountId,
								Answer = answer
							};

							if (answer == null)
							{
								taskInfo.Status = "NotSubmitted";
							}
							else
							{
								if (allReviews.TryGetValue(answer.ReviewIdFk, out var review))
								{
									taskInfo.Status = review.ReviewName ?? "Submitted";
								}
								else
								{
									taskInfo.Status = "Submitted";
								}
							}

							CourseTasks.Add(taskInfo);
						}
						catch (Exception ex)
						{
							var taskInfo = new CourseTaskInfo
							{
								Task = task,
								StudentId = currentUser.AccountId,
								Status = "NotSubmitted",
								Answer = null
							};

							CourseTasks.Add(taskInfo);
						}
					}

					UpdateCourseStatistics(); //обновление статистики курса
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке заданий: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		//КОМАНДЫ НАВИГАЦИИ
		private void ShowCourses() //показать курсы
		{
			ShowCoursesVisibility = Visibility.Visible;
			ShowCourseDetailsVisibility = Visibility.Collapsed;
			CoursesButtonColor = Brushes.LightBlue;
			ProfileButtonColor = Brushes.Transparent;
		}

		private void Logout() //выход из системы
		{
			CurrentUser.Username = null;
			CurrentUser.Role = null;

			Application.Current.Windows.OfType<Views.StudentMainWindow>().First()?.Close();
			new Views.LoginWindow().Show();
		}

		//КОМАНДЫ С КУРСАМИ
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
						//обновление счетчиков курса
						UpdateCourseCounters(CurrentCourseDetails);

						LoadCourseTasksWithStatus(course.CourseId);

						ShowCourseDetailsVisibility = Visibility.Visible;
						ShowCoursesVisibility = Visibility.Collapsed;

						//уведомление UI об обновлении свойств
						OnPropertyChanged(nameof(CurrentCourseDetails));
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при открытии курса: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void BackToCourses() //вернуться к курсам
		{
			//перезагрузка статистики при возврате к списку курсов
			using (var context = new VlasovaAaКурсовая1Context())
			{
				var currentUser = context.Accounts
					.FirstOrDefault(a => a.Username == CurrentUser.Username);

				if (currentUser != null)
				{
					LoadOverallStatistics(currentUser.AccountId);
				}
			}

			ShowCourseDetailsVisibility = Visibility.Collapsed;
			ShowCoursesVisibility = Visibility.Visible;
			CurrentCourseDetails = null;
			CourseTasks.Clear();
		}

		private void RefreshCourses() //обновить курсы
		{
			LoadStudentCourses();
		}

		//КОМАНДЫ С ЗАДАНИЯМИ И ОТВЕТАМИ
		private void SubmitAnswer(CourseTaskInfo taskInfo) //отправить ответ
		{
			if (taskInfo == null) return;

			if (CurrentCourseDetails?.CourseStatusIdFkNavigation?.StatusName == "Archive")
			{
				MessageBox.Show("Невозможно отправить ответ. Курс находится в архиве.", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			SelectedTaskForAnswer = taskInfo;
			AnswerText = taskInfo.Answer?.AnswerText ?? "";
			AnswerDocumentPath = taskInfo.Answer?.AnswerDocument ?? "";

			ShowSubmitModalVisibility = Visibility.Visible;
		}

		private void CancelSubmit() //отменить отправку
		{
			ShowSubmitModalVisibility = Visibility.Collapsed;
			SelectedTaskForAnswer = null;
			AnswerText = "";
			AnswerDocumentPath = "";
		}

		private void ConfirmSubmit() //подтвердить отправку
		{
			try
			{
				if (SelectedTaskForAnswer == null) return;

				if (string.IsNullOrWhiteSpace(AnswerText))
				{
					MessageBox.Show("Введите ответ на задание", "Ошибка",
						MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}

				using (var context = new VlasovaAaКурсовая1Context())
				{
					var currentUser = context.Accounts
						.FirstOrDefault(a => a.Username == CurrentUser.Username);

					if (currentUser == null)
					{
						MessageBox.Show("Пользователь не найден", "Ошибка",
							MessageBoxButton.OK, MessageBoxImage.Error);
						return;
					}

					var submittedStatusId = GetSubmittedStatusId(context);

					var existingAnswer = context.Answers
						.FirstOrDefault(a => a.TaskIdFk == SelectedTaskForAnswer.Task.TaskId &&
											a.EmployeeIdFk == currentUser.AccountId);

					if (existingAnswer != null)
					{
						existingAnswer.AnswerText = AnswerText;
						existingAnswer.AnswerDocument = AnswerDocumentPath;
						existingAnswer.AnswerDate = DateTime.Now;
						existingAnswer.ReviewIdFk = submittedStatusId;
					}
					else
					{
						var answer = new Answer
						{
							TaskIdFk = SelectedTaskForAnswer.Task.TaskId,
							EmployeeIdFk = currentUser.AccountId,
							AnswerText = AnswerText,
							AnswerDocument = AnswerDocumentPath,
							AnswerDate = DateTime.Now,
							ReviewIdFk = submittedStatusId,
							Grade = 0,
							FeedbackIdFk = null
						};

						context.Answers.Add(answer);
					}

					context.SaveChanges();

					MessageBox.Show("Ответ успешно отправлен на проверку", "Успех",
						MessageBoxButton.OK, MessageBoxImage.Information);

					LoadCourseTasksWithStatus(CurrentCourseDetails.CourseId);

					ShowSubmitModalVisibility = Visibility.Collapsed;
					SelectedTaskForAnswer = null;
					AnswerText = "";
					AnswerDocumentPath = "";

					//обновление общей статистики после отправки ответа
					LoadOverallStatistics(currentUser.AccountId);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при отправке ответа: {ex.Message}", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		//ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ
		private int GetSubmittedStatusId(VlasovaAaКурсовая1Context context) //получить ID статуса "Submitted"
		{
			var submittedStatus = context.Reviews
				.FirstOrDefault(r => r.ReviewName == "Submitted" ||
								r.ReviewName == "На проверке" ||
								r.ReviewName == "Submitted (На проверке)");

			if (submittedStatus != null)
				return submittedStatus.ReviewId;

			var firstStatus = context.Reviews.FirstOrDefault();
			if (firstStatus != null)
				return firstStatus.ReviewId;

			throw new InvalidOperationException("Таблица Reviews пустая. Добавьте хотя бы один статус.");
		}

		private void UpdateCourseStatistics() //обновить статистику курса
		{
			//статистика только для текущего курса
			_totalTasks = CourseTasks.Count;
			_completedTasks = CourseTasks.Count(t => t.Status == "Accepted" || t.Status == "Принято");
			_pendingTasks = CourseTasks.Count(t => t.Status == "Submitted" || t.Status == "На проверке");

			OnPropertyChanged(nameof(TotalTasksText));
			OnPropertyChanged(nameof(CompletedTasksText));
			OnPropertyChanged(nameof(PendingTasksText));
		}

		private void UpdateCourseCounters(Course course) //обновить счетчики курса
		{
			try
			{
				using (var context = new VlasovaAaКурсовая1Context())
				{
					course.TasksCount = context.Tasks
						.Count(t => t.CourseIdFk == course.CourseId);

					course.StudentsCount = context.Enrollments
						.Count(e => e.CourseIdFk == course.CourseId &&
								   e.EnrollmentStatusIdFkNavigation.StatusName != "Cancelled");

					OnPropertyChanged(nameof(course.TasksCount));
					OnPropertyChanged(nameof(course.StudentsCount));
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при обновлении счетчиков курса: {ex.Message}");
			}
		}

		//ОБНОВЛЕНИЕ ДАННЫХ
		public void UpdateCourseData() //обновление данных курса
		{
			if (CurrentCourseDetails != null)
			{
				UpdateCourseCounters(CurrentCourseDetails);
				LoadCourseTasksWithStatus(CurrentCourseDetails.CourseId);
			}
		}

		//INOTIFY_PROPERTY_CHANGED
		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propertyName = "") //уведомление об изменении свойства
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}