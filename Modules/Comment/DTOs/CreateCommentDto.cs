using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myapp.Modules.Comment.DTOs
{
    public class CreateCommentDto
    {
        public int PostId { get; set; }
        public string Content { get; set; } 
    }
}