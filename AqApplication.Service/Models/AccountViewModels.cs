using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AqApplication.Service.Models
{
    public class LoginDTO
    {

        public string Username { get; set; }
        public string Password { get; set; }


    }
    public class RegisterDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }

    public class LoginResultModel
    {
        public string Token { get; set; }

        public string UserName { get; set; }
        public string FullName { get; set; }


        public int TotalMark { get; set; }

        public string UserId { get; set; }
    }
}
