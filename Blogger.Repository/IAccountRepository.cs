using System;
using System.Threading;
using System.Threading.Tasks;
using Blogger.Models.Account;
using Microsoft.AspNetCore.Identity;

namespace Blogger.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateAsync(ApplicationUserIdentity user, CancellationToken cancellationToken);

        Task<ApplicationUserIdentity> GetByUsernameAsync(string normalizedUsername, CancellationToken cancellationToken);
    }
}
