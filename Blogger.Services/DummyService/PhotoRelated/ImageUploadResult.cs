using System;

namespace Blogger.Services.DummyService.PhotoRelated
{
    public class ImageUploadResult
    {
        public string PublicId { get; set; }
        public string ImageUrl { get; set; }
        public ImageUploadError Error { get; set; }
    }
}
