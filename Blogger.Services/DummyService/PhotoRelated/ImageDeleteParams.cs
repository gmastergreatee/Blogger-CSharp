using System;

namespace Blogger.Services.DummyService.PhotoRelated
{
    public class ImageDeleteParams
    {
        public string _publicId { get; private set; }

        public ImageDeleteParams(string publicId)
        {
            _publicId = publicId;
        }
    }
}
