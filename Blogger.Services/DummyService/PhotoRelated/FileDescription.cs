using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Blogger.Services.DummyService.PhotoRelated
{
    public class FileDescription
    {
        private Stream _stream;
        private string _fileName;

        public FileDescription(string fileName, Stream stream)
        {
            _fileName = fileName;
            _stream = stream;
        }

        public async Task<byte[]> ReadStreamAsync()
        {
            return Encoding.UTF8.GetBytes(await new StreamReader(_stream).ReadToEndAsync());
        }
    }
}
