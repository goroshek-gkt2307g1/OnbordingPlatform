using System;
using System.Collections.Generic;

namespace OnbordingPlatform.Entities;

public partial class Review
{
    public int ReviewId { get; set; }

    public string ReviewName { get; set; } = null!;

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
}
