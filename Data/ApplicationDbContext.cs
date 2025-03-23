using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using myapp.Modules.User.Models;
using myapp.Modules.Post.Models;
using myapp.Modules.Comment.Models;
using myapp.Modules.Like.Models;

namespace myapp.Data
{
    public class ApplicationDbContext :  IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions) 
        : base(dbContextOptions)
        {

        }

        public DbSet<UserPost> Posts { get; set; }
        public DbSet<UserComment> Comments { get; set; }
        public DbSet<UserLike> Likes { get; set; }

    }


}