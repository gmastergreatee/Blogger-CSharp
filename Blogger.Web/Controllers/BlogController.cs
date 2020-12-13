using System.Linq;
using Blogger.Repository;
using Blogger.Models.Blog;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace Blogger.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IPhotoRepository _photoRepository;

        public BlogController(
            IBlogRepository blogRepository,
            IPhotoRepository photoRepository
        )
        {
            _blogRepository = blogRepository;
            _photoRepository = photoRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Blog>> Create(BlogCreate blogCreate)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

            if (blogCreate.PhotoId.HasValue)
            {
                var photo = await _photoRepository.GetAsync(blogCreate.PhotoId.Value);

                if (photo.ApplicationUserId != applicationUserId)
                {
                    return BadRequest("You did not upload the photo");
                }
            }

            var blog = await _blogRepository.UpsertAsync(blogCreate, applicationUserId);
            return Ok(blog);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResults<Blog>>> GetAll([FromQuery] BlogPaging blogPaging)
        {
            var blogs = await _blogRepository.GetAllAsync(blogPaging);
            return Ok(blogs);
        }

        [HttpGet("{blogId:int}")]
        public async Task<ActionResult<Blog>> GetAction(int blogId)
        {
            var blog = await _blogRepository.GetAsync(blogId);
            return Ok(blog);
        }

        [HttpGet("user/{applicationUserId}")]
        public async Task<ActionResult<List<Blog>>> GetByApplicationUserId(int applicationUserId)
        {
            var blogs = await _blogRepository.GetAllByUserIdAsync(applicationUserId);
            return Ok(blogs);
        }

        [HttpGet("famous")]
        public async Task<ActionResult<List<Blog>>> GetAllFamous()
        {
            var blogs = await _blogRepository.GetAllFamousAsync();
            return Ok(blogs);
        }

        [HttpDelete("{blogId:int}")]
        public async Task<ActionResult<int>> Delete(int blogId)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

            var foundBlog = await _blogRepository.GetAsync(blogId);
            if (foundBlog != null)
            {
                if (foundBlog.ApplicationUserId == applicationUserId)
                {
                    var affectedRows = await _blogRepository.DeleteAsync(blogId);
                    return Ok(affectedRows);
                }
                return BadRequest("You did not create this blog");
            }
            return BadRequest("Blog doesn't exist");
        }
    }
}
