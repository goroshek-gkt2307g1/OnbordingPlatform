using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnbordingPlatform.Models
{
    public static class CurrentUser
    {
        public static string Username { get; set; }
        public static string Role { get; set; }
        public static int Id { get; set; }
        public static bool IsLoggedIn => !string.IsNullOrEmpty(Username);
        public static bool IsAdmin => Role == "Admin";
        public static bool IsMentor => Role == "Mentor";
        public static bool IsStudent => Role == "Student";
    }

}
