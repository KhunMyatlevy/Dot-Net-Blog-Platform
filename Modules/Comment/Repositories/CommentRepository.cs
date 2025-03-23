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

        public async Task<UserComment> DeleteById(int id)
        {
            var existing_comment = await _context.Comments.FirstOrDefaultAsync(u => u.Id == id);
            if (existing_comment == null)
            {
                return null;
            }

            _context.Comments.Remove(existing_comment);
            await _context.SaveChangesAsync();
            return existing_comment;
        }

        public async Task<List<UserComment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<UserComment> GetById(int id)
        {
            return await _context.Comments.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserComment> UpdateById(UpdateCommentDto updateCommentDto, int id)
        {
            var existing_comment = await _context.Comments.FirstOrDefaultAsync(u => u.Id == id);

            if (existing_comment == null)
            {
                return null;
            }

            existing_comment.Content = updateCommentDto.Content;
            await _context.SaveChangesAsync();

            return existing_comment;
        }


    }
}