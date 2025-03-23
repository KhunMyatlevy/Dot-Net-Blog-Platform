using Microsoft.EntityFrameworkCore;
using myapp.Data;
using myapp.Modules.Comment.Interface;
using myapp.Modules.Like.Interface;
using myapp.Modules.Like.Models;
using myapp.Modules.Post.Interface;

namespace myapp.Modules.Like.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        public LikeRepository(ApplicationDbContext context,
                                IPostRepository postRepository, 
                                ICommentRepository commentRepository)
        {
            _context = context;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        public async Task<UserLike> CreateAsync(string userid, int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            var comment = await _commentRepository.GetById(id);

            int? postId = null;
            int? commentId = null;

            if (post != null)
            {
                // It's a Post ID
                postId = post.Id; 

                var likeonpost = new UserLike
                {
                    UserId = userid,
                    PostId = postId,
                    CommentId = null,
                    CreatedAt = DateTime.UtcNow,
                };

                await _context.Likes.AddAsync(likeonpost);
                await _context.SaveChangesAsync();
                return likeonpost;
            }
            else if (comment != null)
            {
                // It's a Comment ID
                commentId = comment.Id; 
                var likeoncomment = new UserLike
                {
                    UserId = userid,
                    PostId = null,
                    CommentId = commentId,
                    CreatedAt = DateTime.UtcNow,
                };

                await _context.Likes.AddAsync(likeoncomment);
                await _context.SaveChangesAsync();
                return likeoncomment;
            }
            else
            {
                return null;
            }
        }

        public async Task<UserLike> DeleteByIdAsync(int id)
        {
            var existing_Like = await _context.Likes.FirstOrDefaultAsync(u => u.Id == id);
            if (existing_Like == null)
            {
               return null;
            }

            _context.Likes.Remove(existing_Like);
            await _context.SaveChangesAsync();
            return existing_Like;
        }

        public async Task<List<UserLike>> GetAllAsync()
        {
            return await _context.Likes.ToListAsync();
        }

        public async Task<UserLike> GetByIdAsync(int id)
        {
            var like = await _context.Likes.FirstOrDefaultAsync(u => u.Id == id);

            if (like == null)
            {
                return null;
            }

            return like;
        }
    }
}