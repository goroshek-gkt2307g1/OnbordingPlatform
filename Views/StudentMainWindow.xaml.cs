using System.Windows;

namespace OnbordingPlatform.Views
{
    public partial class StudentMainWindow : Window
    {
        public StudentMainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.StudentMainViewModel();
        }
    }
}