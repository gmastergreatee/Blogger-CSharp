using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Blogger.Services.DummyService.PhotoRelated
{
    public class DummyPhotoServer
    {
        public async Task<ImageUploadResult> UploadAsync(ImageUploadParams imageUploadParams)
        {
            string newFileName = "";
            try
            {
                var folder = Directory.GetCurrentDirectory();
                var dirName = "BlogImages";

                var targetFolder = Path.Combine(folder, dirName);

                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }

                newFileName = Guid.NewGuid().ToString();
                var filePath = Path.Combine(targetFolder, newFileName);

                File.WriteAllBytes(filePath, await imageUploadParams.File.ReadStreamAsync());
            }
            catch (Exception ex)
            {
                return new ImageUploadResult()
                {
                    Error = new ImageUploadError()
                    {
                        Message = JsonConvert.SerializeObject(ex)
                    }
                };
            }

            return new ImageUploadResult()
            {
                PublicId = newFileName,
                ImageUrl = "api/Photo/GetPic?id=" + newFileName,
                Error = null,
            };
        }

        public async Task<ImageDeleteResult> DestroyAsync(ImageDeleteParams deletionParams)
        {
            try
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
            }
            catch (Exception ex)
            {
                return new ImageDeleteResult()
                {
                    Error = new ImageDeleteError() { Message = JsonConvert.SerializeObject(ex) }
                };
            }

            return new ImageDeleteResult();
        }
    }
}
