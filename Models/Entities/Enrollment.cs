using System;
using System.Collections.Generic;

namespace OnbordingPlatform.Entities;

public partial class Enrollment
{
    public int EnrollmentId { get; set; }

    public DateTime? EnrollmentDate { get; set; }

    public int AccountIdFk { get; set; }

    public int CourseIdFk { get; set; }

    public int EnrollmentStatusIdFk { get; set; }

    public virtual Account AccountIdFkNavigation { get; set; } = null!;

    public virtual Course CourseIdFkNavigation { get; set; } = null!;

    public virtual EnrollmentStatus EnrollmentStatusIdFkNavigation { get; set; } = null!;
}
