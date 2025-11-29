using OnbordingPlatform.ViewModels;
using System.Windows;

namespace OnbordingPlatform.Views
{
    public partial class AdminMainWindow : Window
    {
        public AdminMainWindow()
        {
            InitializeComponent();
            var viewModel = new AdminMainViewModel();
            DataContext = viewModel;
        }
    }
}