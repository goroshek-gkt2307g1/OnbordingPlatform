using System;
using System.Collections.Generic;

namespace OnbordingPlatform.Entities;

public partial class Task
{
    public int TaskId { get; set; }

    public string TaskTitle { get; set; } = null!;

    public string TaskDescription { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public string? TaskDocument { get; set; }

    public DateOnly DueDate { get; set; }

    public int CourseIdFk { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual Course CourseIdFkNavigation { get; set; } = null!;
}
