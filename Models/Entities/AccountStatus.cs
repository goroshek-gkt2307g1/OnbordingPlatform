using System;
using System.Collections.Generic;

namespace OnbordingPlatform.Entities;

public partial class AccountStatus
{
    public int AccountStatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public static AccountStatus[] SeedData => new[]
{
        new AccountStatus { StatusName = "Active" },
        new AccountStatus { StatusName = "Inactive" }
    };

}
