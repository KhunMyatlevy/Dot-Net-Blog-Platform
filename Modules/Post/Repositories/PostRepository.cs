using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using myapp.Data;
using myapp.Modules.Post.DTOs;
using myapp.Modules.Post.Interface;
using myapp.Modules.Post.Models;

namespace myapp.Modules.Post.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<UserPost> CreateAsync(UserPost post)
        {


            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return post;
    }

        public async Task<UserPost> DeleteByIdAsync(int id)
        {
            var existingPost = await _context.Posts.FirstOrDefaultAsync(u => u.Id == id);

            if (existingPost == null)
            {
                return null;
            }

            _context.Posts.Remove(existingPost);
            await _context.SaveChangesAsync();
            return existingPost;
        }

        public async Task<List<UserPost>> GetAllAsync()
        {
            return await _context.Posts.ToListAsync();
        }


        public async Task<UserPost> GetByIdAsync(int id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(u => u.Id == id);
            if (post == null)
            {
                return null;
            }
            return post;
        }

        public async Task<UserPost> UpdateByIdAsync(UpdatePostDto updatePostDto, int id)
        {
            var existingPost = await _context.Posts.FirstOrDefaultAsync(u => u.Id == id);

            if (existingPost == null)
            {
                return null;
            }

            existingPost.Title = updatePostDto.Title;
            existingPost.Content = updatePostDto.Content;
            await _context.SaveChangesAsync();

            return existingPost;
        }
    }
}