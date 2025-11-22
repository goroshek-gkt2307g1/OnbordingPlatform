using System;
using System.Collections.Generic;

namespace OnbordingPlatform.Entities;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public string? FeedbackText { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int MentorIdFk { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual Account MentorIdFkNavigation { get; set; } = null!;
}
