using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using myapp.Modules.Comment.Models;
using myapp.Modules.Post.Models;
using myapp.Modules.User.Models;

namespace myapp.Modules.Like.Models
{
    public class UserLike
    {
        [Key]
        public int Id { get; set; } // Primary Key

        [Required]
        public string UserId { get; set; } // Who liked
        public int? PostId { get; set; } // Nullable if it's a comment like
        public int? CommentId { get; set; } // Nullable if it's a post like

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Timestamp when liked
        public virtual AppUser User { get; set; }
        public virtual UserPost Post { get; set; }
        public virtual UserComment Comment { get; set; }
    }
}