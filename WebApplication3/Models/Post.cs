using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class Post
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Body { get; set; }

    public int? UserId { get; set; }

    public string? Status { get; set; }

    public string? CreatedAt { get; set; }

    public virtual User? User { get; set; }
}
