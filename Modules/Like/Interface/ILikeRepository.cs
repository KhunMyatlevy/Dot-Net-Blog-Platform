using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using myapp.Modules.Like.Models;

namespace myapp.Modules.Like.Interface
{
    public interface ILikeRepository
    {
        Task<UserLike> CreateAsync(string userid, int id);
        Task<List<UserLike>> GetAllAsync();
        Task<UserLike> GetByIdAsync(int id);
        Task<UserLike> DeleteByIdAsync(int id);
    }
}