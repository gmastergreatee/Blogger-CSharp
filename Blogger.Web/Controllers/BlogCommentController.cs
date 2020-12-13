using System.Linq;
using Blogger.Repository;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blogger.Models.BlogComment;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace Blogger.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogCommentController : ControllerBase
    {
        private readonly IBlogCommentRepository _blogCommentRepository;

        public BlogCommentController(
            IBlogCommentRepository blogCommentRepository
        )
        {
            _blogCommentRepository = blogCommentRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BlogComment>> Create(BlogCommentCreate blogCommentCreate)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

            var createdBlogComment = await _blogCommentRepository.UpsertAsync(blogCommentCreate, applicationUserId);
            return Ok(createdBlogComment);
        }

        [HttpGet("{blogId:int}")]
        public async Task<ActionResult<List<BlogComment>>> GetAll(int blogId)
        {
            var blogComments = await _blogCommentRepository.GetAllAsync(blogId);
            return Ok(blogComments);
        }

        [Authorize]
        [HttpDelete("{blogCommentId:int}")]
        public async Task<ActionResult<int>> Delete(int blogCommentId)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

            var foundBlogComment = await _blogCommentRepository.GetAsync(blogCommentId);
            if (foundBlogComment != null)
            {
                if (foundBlogComment.ApplicationUserId == applicationUserId)
                {
                    var affectedRows = await _blogCommentRepository.DeleteAsync(blogCommentId);
                    return Ok(affectedRows);
                }
                return BadRequest("This comment was not created by you");
            }
            return BadRequest("Comment does not exist");
        }
    }
}
