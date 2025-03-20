using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
                CreatedAt = createPostDto.CreatedAt,
                UserId = decryptedUserId, // Set the user ID
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
    }
}