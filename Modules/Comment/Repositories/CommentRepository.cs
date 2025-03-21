using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using myapp.Data;
using myapp.Modules.Comment.DTOs;
using myapp.Modules.Comment.Interface;
using myapp.Modules.Comment.Models;

namespace myapp.Modules.Comment.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<UserComment> Create(UserComment userComment)
        {
            
            await _context.Comments.AddAsync(userComment);
            await _context.SaveChangesAsync();
            return userComment;
        }

        public async Task<List<UserComment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }
    }
}