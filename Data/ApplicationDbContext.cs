using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using myapp.Modules.User.Models;
using myapp.Modules.Post.Models;

namespace myapp.Data
{
    public class ApplicationDbContext :  IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions) 
        : base(dbContextOptions)
        {

        }

        public DbSet<UserPost> Posts { get; set; }

    }


}