using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Blogger.Services.DummyService.PhotoRelated;

namespace Blogger.Services
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        Task<ImageDeleteResult> DeletePhotoAsync(string publicId);
    }
}
