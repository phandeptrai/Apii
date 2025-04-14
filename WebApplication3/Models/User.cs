using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? CreatedAt { get; set; }

    public string? Email { get; set; }

    public DateTime? Birthday { get; set; }

    public int? ProvinceId { get; set; }

    public virtual ICollection<Follow> Followers { get; set; } = new List<Follow>();     // Ai đang theo dõi user này
    public virtual ICollection<Follow> Following { get; set; } = new List<Follow>();     // User này đang theo dõi ai


    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual Province? Province { get; set; }
}
