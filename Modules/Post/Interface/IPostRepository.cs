using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using myapp.Modules.Post.DTOs;
using myapp.Modules.Post.Models;

namespace myapp.Modules.Post.Interface
{
    public interface IPostRepository
    {
        Task<UserPost> CreateAsync (UserPost post);
        Task<UserPost> GetByIdAsync (int id);
        Task<List<UserPost>> GetAllAsync ();
        Task<UserPost> UpdateByIdAsync (UpdatePostDto updatePostDto, int id);
        Task<UserPost> DeleteByIdAsync (int id);
    }
}