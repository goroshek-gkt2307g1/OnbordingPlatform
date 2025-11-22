using System;
using System.Collections.Generic;

namespace OnbordingPlatform.Entities;

public partial class Account
{
    public int AccountId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public int RoleIdFk { get; set; }

    public string? AccountDescription { get; set; }

    public DateOnly HireDate { get; set; }

    public int AccountStatusFk { get; set; }

    public virtual AccountStatus AccountStatusFkNavigation { get; set; } = null!;

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual Role RoleIdFkNavigation { get; set; } = null!;
}
