using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using myapp.Modules.User.Models;

namespace myapp.Modules.Post.Models
{
    public class UserPost
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UserId { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}