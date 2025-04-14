using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class Province
{
    public int IdProvince { get; set; }

    public string? NameProvince { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
