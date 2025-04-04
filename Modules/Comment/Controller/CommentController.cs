using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myapp.Modules.Comment.DTOs;
using myapp.Modules.Comment.Interface;
using myapp.Modules.Comment.Models;
using myapp.Modules.Post.Interface;
using myapp.Modules.User.Interface;

namespace myapp.Modules.Comment.Controller
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IPostRepository _postRepo;
        private readonly IEncryptionService _encryptionService;
        public CommentController(ICommentRepository commentRepo, IPostRepository postRepo, IEncryptionService encryptionService)
        {
            _commentRepo = commentRepo;
            _postRepo = postRepo;
            _encryptionService = encryptionService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create ([FromBody] CreateCommentDto createCommentDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Invlaid request data");
            }

            int postid = createCommentDto.PostId;

            var existing_post = await _postRepo.GetByIdAsync(postid);

            if (existing_post == null)
            {
                return NotFound("Post Do not found");
            }
            var encryptedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (encryptedUserId == null)
            {
                return Unauthorized("Invalid token.");
            }

            string decryptedUserId = _encryptionService.DecryptData(encryptedUserId);
            var userComment = new UserComment
            {
                PostId = createCommentDto.PostId,
                Content = createCommentDto.Content,
                UserId = decryptedUserId, // Assuming the user is logged in and you get their ID from the User object
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _commentRepo.Create(userComment);

            return Ok(new { message = "Comment created successfully." });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll ()
        {
            var comments = await _commentRepo.GetAllAsync();
            if (!comments.Any())
            {
                return BadRequest("No comments available");
            }

            return Ok(comments);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GeyById(int id)
        {
            var comment = await _commentRepo.GetById(id);

            if (comment == null)
            {
                return BadRequest("Invalid Id");
            }

            return Ok(comment);
        }

        
        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateById ([FromBody] UpdateCommentDto updateCommentDto, int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }

            var updatedcomment = await _commentRepo.UpdateById(updateCommentDto, id);

            if(updatedcomment == null)
            {
                return NotFound("Comment Not found.");
            }

            return Ok(updatedcomment);

        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteById (int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }
            var existing_comment = await _commentRepo.DeleteById(id);

            if (existing_comment == null)
            {
                return NotFound("Comment not found.");
            }

            return Ok("Comment deleted successfully.");


        }
    }
}