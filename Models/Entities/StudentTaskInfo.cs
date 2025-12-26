using OnbordingPlatform.Entities;
using System.ComponentModel;

namespace OnbordingPlatform.ViewModels
{
    public class StudentTaskInfo : INotifyPropertyChanged
    {
        private Entities.Task _task;
        private Answer _answer;
        private string _status;

        public Entities.Task Task
        {
            get => _task;
            set
            {
                _task = value;
                OnPropertyChanged();
            }
        }

        public Answer Answer
        {
            get => _answer;
            set
            {
                _answer = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasAnswer));
                OnPropertyChanged(nameof(StudentAnswer));
                OnPropertyChanged(nameof(AnswerText));
                OnPropertyChanged(nameof(ShowAcceptButton));
                OnPropertyChanged(nameof(ShowRejectButton));
                OnPropertyChanged(nameof(ShowRevokeButton));
            }
        }

        public int StudentId { get; set; }

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShowAcceptButton));
                OnPropertyChanged(nameof(ShowRejectButton));
                OnPropertyChanged(nameof(ShowRevokeButton));
            }
        }

        public string AnswerText => Answer?.AnswerText ?? "Ответ не предоставлен";

        public bool HasAnswer => Answer != null;

        public bool ShowAcceptButton => Status == "Submitted" || Status == "На проверке";
        public bool ShowRejectButton => Status == "Submitted" || Status == "На проверке";
        public bool ShowRevokeButton => Status == "Accepted" || Status == "Rejected" ||
                                       Status == "Принято" || Status == "Отклонено";

        public string StatusDisplay
        {
            get
            {
                return Status switch
                {
                    "NotSubmitted" => "Не отправлено",
                    "Submitted" => "На проверке",
                    "Accepted" => "Принято",
                    "Rejected" => "Отклонено",
                    "Returned" => "Возвращено",
                    _ => Status
                };
            }
        }

        public string StudentAnswer => Answer?.AnswerText ?? "";

        public bool ShowNoAnswerText => !HasAnswer;
        public string AnswerPreview => HasAnswer && !string.IsNullOrEmpty(StudentAnswer)
            ? (StudentAnswer.Length > 50 ? StudentAnswer.Substring(0, 50) + "..." : StudentAnswer)
            : "Нет ответа";

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}