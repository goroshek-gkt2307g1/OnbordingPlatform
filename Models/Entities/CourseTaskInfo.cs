using OnbordingPlatform.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnbordingPlatform.Models.Entities
{
    public class CourseTaskInfo : INotifyPropertyChanged
    {
        private OnbordingPlatform.Entities.Task _task;
        private Answer _answer;
        private string _status;

        public OnbordingPlatform.Entities.Task Task
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
                OnPropertyChanged(nameof(CanSubmit));
                OnPropertyChanged(nameof(CanResubmit));
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
                OnPropertyChanged(nameof(StatusDisplay));
                OnPropertyChanged(nameof(CanSubmit));
                OnPropertyChanged(nameof(CanResubmit));
            }
        }

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

        public bool HasAnswer => Answer != null;
        public bool CanSubmit => Status == "NotSubmitted" || Status == "Returned";
        public bool CanResubmit => Status == "Rejected" || Status == "Returned";

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
