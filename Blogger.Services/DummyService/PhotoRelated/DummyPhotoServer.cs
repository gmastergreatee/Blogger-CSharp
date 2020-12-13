using System;
using System.IO;
using System.Threading.Tasks;

namespace Blogger.Services.DummyService.PhotoRelated
{
    public class DummyPhotoServer
    {
        public async Task<ImageUploadResult> UploadAsync(ImageUploadParams imageUploadParams)
        {
            var folder = Directory.GetCurrentDirectory();
            var dirName = "BlogImages";

            var targetFolder = Path.Combine(folder, dirName);

            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            string newFileName = Guid.NewGuid().ToString();
            var filePath = Path.Combine(targetFolder, newFileName);

            File.WriteAllBytes(filePath, await imageUploadParams.File.ReadStreamAsync());

            return new ImageUploadResult()
            {
                PublicId = newFileName
            };
        }

        public async Task<DeletionResult> DestroyAsync(DeletionParams deletionParams)
        {
            var folder = Directory.GetCurrentDirectory();
            var dirName = "BlogImages";

            var targetFolder = Path.Combine(folder, dirName);

            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            await Task.Run(new Action(() =>
            {
                var filePath = Path.Combine(targetFolder, deletionParams._publicId);
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }));

            return new DeletionResult();
        }
    }
}
