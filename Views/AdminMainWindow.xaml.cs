using OnbordingPlatform.ViewModels;
using System.Windows;

namespace OnbordingPlatform.Views
{
    public partial class AdminMainWindow : Window
    {
        public AdminMainWindow()
        {
            InitializeComponent();
            DataContext = new AdminMainViewModel();

            this.Closed += (s, e) => Application.Current.Shutdown();
        }
    }
}