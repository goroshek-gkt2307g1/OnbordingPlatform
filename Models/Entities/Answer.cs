using System;
using System.Collections.Generic;

namespace OnbordingPlatform.Entities;

public partial class Answer
{
    public int AnswerId { get; set; }

    public DateTime? AnswerDate { get; set; }

    public int TaskIdFk { get; set; }

    public int EmployeeIdFk { get; set; }

    public string AnswerText { get; set; } = null!;

    public string? AnswerDocument { get; set; }

    public int Grade { get; set; }

    public int? FeedbackIdFk { get; set; }

    public int ReviewIdFk { get; set; }

    public virtual Account EmployeeIdFkNavigation { get; set; } = null!;

    public virtual Feedback? FeedbackIdFkNavigation { get; set; }

    public virtual Review ReviewIdFkNavigation { get; set; } = null!;

    public virtual Task TaskIdFkNavigation { get; set; } = null!;
}
