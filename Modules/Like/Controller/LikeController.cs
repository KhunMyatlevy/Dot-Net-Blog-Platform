using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myapp.Modules.Comment.Interface;
using myapp.Modules.Like.Interface;
using myapp.Modules.Like.Models;
using myapp.Modules.Post.Interface;
using myapp.Modules.User.Interface;

namespace myapp.Modules.Like.Controller
{
    [Route("api/like")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeRepository _likeRepo;
        private readonly IEncryptionService _encryptionService;

        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
           
        public LikeController(ILikeRepository likeRepo, 
                                IEncryptionService encryptionService, 
                                IPostRepository postRepository, 
                                ICommentRepository commentRepository)
        {
            _likeRepo = likeRepo;
            _encryptionService = encryptionService;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        [HttpPost("{PostOrCommentId:int}")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] int PostOrCommentId)
        {
            var encryptedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (encryptedUserId == null)
            {
                return Unauthorized("Invalid token.");
            }

            string decryptedUserId = _encryptionService.DecryptData(encryptedUserId);

            if (decryptedUserId == null)
            {
                return Unauthorized("User is not authenticated");
            }

            var like = await _likeRepo.CreateAsync(decryptedUserId, PostOrCommentId);
            if (like == null)
            {
                return NotFound("Post or Comment not found.");
            }

            return Ok(like);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var likes = await _likeRepo.GetAllAsync();

            if (likes == null)
            {
                return NotFound("Like record not found.");
            }
            return Ok(likes);
        }
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var like = await _likeRepo.GetByIdAsync(id);

            if (like == null)
            {
                return NotFound("Like Record not found");
            }

            return Ok(like);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteById(int id)
        {
            var existing_Like = await _likeRepo.DeleteByIdAsync(id);
            if (existing_Like == null)
            {
                return NotFound("Like record not found.");
            }

            return NoContent();
        }
    }
}