using System;
using System.Collections.Generic;

namespace OnbordingPlatform.Entities;

public partial class RolePermission
{
    public int RolePermissionId { get; set; }

    public int RoleIdFk { get; set; }

    public int PermissionIdFk { get; set; }

    public virtual Permission PermissionIdFkNavigation { get; set; } = null!;

    public virtual Role RoleIdFkNavigation { get; set; } = null!;
}
