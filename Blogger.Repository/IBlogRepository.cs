using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blogger.Models.Blog;

namespace Blogger.Repository
{
    public interface IBlogRepository
    {
        Task<Blog> UpsertAsync(BlogCreate blogCreate, int applicationUserId);

        Task<PagedResults<Blog>> GetAllAsync(BlogPaging blogPaging);

        Task<Blog> GetAsync(int blogId);

        Task<List<Blog>> GetAllByUserIdAsync(int applicationUserId);

        Task<List<Blog>> GetAllFamousAsync();

        Task<int> DeleteAsync(int blogId);
    }
}
