using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using myapp.Modules.Comment.DTOs;
using myapp.Modules.Comment.Models;

namespace myapp.Modules.Comment.Interface
{
    public interface ICommentRepository
    {
        Task<UserComment>Create(UserComment userComment);
        Task<List<UserComment>>GetAllAsync ();
        Task<UserComment> GetById (int id);
        Task<UserComment> UpdateById (UpdateCommentDto updateCommentDto, int id);
        Task<UserComment> DeleteById(int id);
    }
}