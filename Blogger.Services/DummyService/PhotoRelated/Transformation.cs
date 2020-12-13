using System;

namespace Blogger.Services.DummyService.PhotoRelated
{
    public class Transformation
    {
        private int _height = 0;
        private int _width = 0;
        private string _cropMode = "";

        public Transformation Height(int height)
        {
            _height = height;
            return this;
        }

        public Transformation Width(int width)
        {
            _width = width;
            return this;
        }

        public Transformation Crop(string cropMode)
        {
            _cropMode = cropMode;
            return this;
        }
    }
}
