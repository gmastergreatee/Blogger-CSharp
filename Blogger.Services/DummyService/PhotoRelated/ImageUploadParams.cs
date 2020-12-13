using System;

namespace Blogger.Services.DummyService.PhotoRelated
{
    public class ImageUploadParams
    {
        public FileDescription File { get; set; }
        public Transformation Transformation { get; set; }
    }
}
