using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Blogger.Services.DummyService.PhotoRelated;

namespace Blogger.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly DummyPhotoServer _dummyPhotoServer;

        public PhotoService()
        {
            _dummyPhotoServer = new DummyPhotoServer();
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Height(300).Width(300).Crop("fill")
                    };

                    uploadResult = await _dummyPhotoServer.UploadAsync(uploadParams);
                }
            }
            return uploadResult;
        }

        public async Task<ImageDeleteResult> DeletePhotoAsync(string publicId)
        {
            var deletionParams = new ImageDeleteParams(publicId);
            var result = await _dummyPhotoServer.DestroyAsync(deletionParams);
            return result;
        }
    }
}
