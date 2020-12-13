using System;
using Blogger.Models.Account;

namespace Blogger.Services
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUserIdentity user);
    }
}
