using System.IO;
using System.Linq;
using Blogger.Services;
using Blogger.Repository;
using Blogger.Models.Photo;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace Blogger.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly IPhotoService _photoService;

        public PhotoController(
            IPhotoRepository photoRepository,
            IBlogRepository blogRepository,
            IPhotoService photoService
            )
        {
            _photoRepository = photoRepository;
            _blogRepository = blogRepository;
            _photoService = photoService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Photo>> UploadPhoto(IFormFile file)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

            var uploadResult = await _photoService.AddPhotoAsync(file);
            if (uploadResult.Error != null)
                return BadRequest(uploadResult.Error.Message);

            var photoCreate = new PhotoCreate()
            {
                PublicId = uploadResult.PublicId,
                ImageUrl = uploadResult.ImageUrl,
                Description = file.FileName,
            };

            var photo = await _photoRepository.InsertAsync(photoCreate, applicationUserId);
            return Ok(photo);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Photo>>> GetByApplicationUserId()
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

            var photos = await _photoRepository.GetAllByUserIdAsync(applicationUserId);
            return Ok(photos);
        }

        [HttpGet("{photoId:int")]
        public async Task<ActionResult<Photo>> GetAction(int photoId)
        {
            var photo = await _photoRepository.GetAsync(photoId);
            return Ok(photo);
        }

        [Authorize]
        [HttpDelete("{photoId:int}")]
        public async Task<ActionResult<int>> Delete(int photoId)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

            var foundPhoto = await _photoRepository.GetAsync(photoId);
            if (foundPhoto != null)
            {
                if (foundPhoto.ApplicationUserId == applicationUserId)
                {
                    var blogs = await _blogRepository.GetAllByUserIdAsync(applicationUserId);
                    var usedInBlog = blogs.Any(a => a.PhotoId == photoId);
                    if (usedInBlog)
                    {
                        return BadRequest("Cannpot remove photo as it is being used in published blog/s.");
                    }

                    var deleteResult = await _photoService.DeletePhotoAsync(foundPhoto.PublicId);
                    if (deleteResult.Error != null) return BadRequest(deleteResult.Error);

                    var affectedRows = await _photoRepository.DeleteAsync(foundPhoto.PhotoId);

                    return Ok(affectedRows);
                }

                return BadRequest("Photo was not uploaded by the current user");
            }

            return BadRequest("Photo does not exist");
        }

        #region Dummy Photo Server
        [HttpGet("{id}")]
        public ActionResult GetPic(string id)
        {
            var folder = Directory.GetCurrentDirectory();
            var dirName = "BlogImages";

            var targetFolder = Path.Combine(folder, dirName);

            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            var filePath = Path.Combine(targetFolder, id);
            if (System.IO.File.Exists(filePath))
                return File(filePath, "image/png");

            return NotFound();
        }
        #endregion
    }
}
