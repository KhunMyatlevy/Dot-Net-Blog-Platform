using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using myapp.Modules.Post.Models;
using myapp.Modules.User.Models;

namespace myapp.Modules.Comment.Models
{
    public class UserComment
    {
        public int Id { get; set; } // Unique identifier for each comment
        public int PostId { get; set; } // Foreign key to Post
        public string UserId { get; set; } // Foreign key to User
        public string Content { get; set; } // Content of the comment
        public DateTime CreatedAt { get; set; } // Timestamp when the comment was created
        public DateTime UpdatedAt { get; set; } // Timestamp when the comment was last updated

        // Navigation properties (if needed for eager loading)
        public virtual UserPost Post { get; set; } // Navigation to Post
        public virtual AppUser User { get; set; } // Navigation to User
    }
}