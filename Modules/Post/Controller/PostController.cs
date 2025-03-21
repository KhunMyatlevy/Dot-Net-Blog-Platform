using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using myapp.Data;
using myapp.Modules.Post.DTOs;
using myapp.Modules.Post.Interface;
using myapp.Modules.Post.Models;
using myapp.Modules.User.Interface;

namespace myapp.Modules.Post.Controller
{
    [Route("api/post")]
    [ApiController]

    public class PostController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEncryptionService _encryptionService;
        private readonly IPostRepository _postRepo;
        public PostController(ApplicationDbContext context, IEncryptionService encryptionService, IPostRepository postRepo)
        {
            _context = context;
            _encryptionService = encryptionService;
            _postRepo = postRepo;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create ([FromBody] CreatePostDto createPostDto)
        {
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid creditientals");
            }

            var encryptedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Console.WriteLine($"Encrypted User ID: {encryptedUserId}");

            if (encryptedUserId == null)
            {
                return Unauthorized("Invalid token.");
            }

            string decryptedUserId = _encryptionService.DecryptData(encryptedUserId);

            var post = new UserPost
            {
                Title = createPostDto.Title,
                Content = createPostDto.Content,
                CreatedAt = DateTime.UtcNow,
                UserId = decryptedUserId,
            };

            var createdPost = await _postRepo.CreateAsync(post);

            return CreatedAtAction(nameof(GetById), new { id = createdPost.Id }, createdPost);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById ([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            return BadRequest(ModelState);

            var post = await _postRepo.GetByIdAsync(id);
            if (post == null)
            {
                return BadRequest("Post not founded");
            }

            return Ok(post);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll ()
        {
            var posts = await _postRepo.GetAllAsync();

            if (!posts.Any())
            {
                return BadRequest("No posts available");
            }

            return Ok(posts);
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateById([FromBody] UpdatePostDto updatePostDto, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }

            var updatedpost = await _postRepo.UpdateByIdAsync(updatePostDto, id);

            if (updatedpost == null)
            {
                return NotFound("Post not found.");
            }

            return Ok(updatedpost);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteById (int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invlaid id");
            }

            var existingPost = await _postRepo.DeleteByIdAsync(id);

            if (existingPost == null)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}