// Views/MentorMainWindow.xaml.cs
using OnbordingPlatform.ViewModels;
using System.Windows;

namespace OnbordingPlatform.Views
{
    public partial class MentorMainWindow : Window
    {
        public MentorMainWindow()
        {
            InitializeComponent();
            DataContext = new MentorMainViewModel();
        }
    }
}