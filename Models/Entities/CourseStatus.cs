using System;
using System.Collections.Generic;

namespace OnbordingPlatform.Entities;

public partial class CourseStatus
{
    public int CourseStatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    public static CourseStatus[] SeedData => new[]
{
        new CourseStatus { StatusName = "Publish" },
        new CourseStatus { StatusName = "Archive" }
    };
}
