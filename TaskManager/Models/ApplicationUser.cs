using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using TaskManager.Domain.Entities;

namespace TaskManager.Models { 
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    }
}