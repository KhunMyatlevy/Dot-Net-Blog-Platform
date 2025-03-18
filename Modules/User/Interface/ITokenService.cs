using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using myapp.Modules.User.Models;

namespace myapp.Modules.User.Interface
{
    public interface ITokenService
    {
         string CreateToken(AppUser appUser);
    }
}