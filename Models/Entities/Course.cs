using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnbordingPlatform.Entities;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseTitle { get; set; } = null!;

    public string CourseDescription { get; set; } = null!;

    public DateTime? CreationDate { get; set; }

    public int MentorIdFk { get; set; }

    public int CourseStatusIdFk { get; set; }

    public virtual CourseStatus CourseStatusIdFkNavigation { get; set; } = null!;

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Account MentorIdFkNavigation { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    [NotMapped] // Указываем, что это свойство не маппится в БД
    public int StudentsCount { get; set; }

    [NotMapped]
    public int TasksCount { get; set; }

    [NotMapped]
    public string StudentsCountText => StudentsCount.ToString();

    [NotMapped]
    public string TasksCountText => TasksCount.ToString();
}
