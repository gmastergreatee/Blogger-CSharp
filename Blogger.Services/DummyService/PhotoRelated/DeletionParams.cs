using System;

namespace Blogger.Services.DummyService.PhotoRelated
{
    public class DeletionParams
    {
        public string _publicId { get; private set; }

        public DeletionParams(string publicId)
        {
            _publicId = publicId;
        }
    }
}
