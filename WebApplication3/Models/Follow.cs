using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class Follow
{
    public int FollowingUserId { get; set; }

    public int FollowedUserId { get; set; }

    public string? CreatedAt { get; set; }

    public virtual User FollowedUser { get; set; } = null!;

    public virtual User FollowingUser { get; set; } = null!;
}
