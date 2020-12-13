using System;
using Blogger.Models.Photo;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Blogger.Repository
{
    public interface IPhotoRepository
    {
        Task<Photo> InsertAsync(PhotoCreate photoCreate, int applicationUserId);

        Task<Photo> GetAsync(int photoId);

        Task<List<Photo>> GetAllByUserIdAsync(int applicationUserId);

        Task<int> DeleteAsync(int photoId);
    }
}
