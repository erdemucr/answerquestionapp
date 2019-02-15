using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AqApplication.Service.Utilities;
using AqApplication.Service.Models;
using AqApplication.Entity.Identity.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AqApplication.Service.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private const string LocalLoginProvider = "Local";
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AqApplication.Repository.Session.IUser _iUser;
        private readonly AppSettings _appSettings;


        public AccountController(UserManager<ApplicationUser> userManager,
                                AqApplication.Repository.Session.IUser iUser,
                                IOptions<AppSettings> appSettings
            )
        {
            _userManager = userManager;
            _iUser = iUser;
            _appSettings = appSettings.Value;
        }


        // GET api/Account/UserInfo
        [HttpGet]
        [Route("UserInfo")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public ActionResult<UserInfoViewModel> GetUserInfo()
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            var user = _iUser.GetUserInfo(HttpContextUserInfo.GetUserId(HttpContext.User.Identity));
            if (!user.Success)
                return NotFound();
            return Ok(new UserInfoViewModel
            {
                Email = user.Data.Email,
                FirstName = user.Data.FirstName,
                LastName = user.Data.LastName
            });
        }
        [HttpGet]
        [Route("CreateAdmin")]
        public ActionResult<IdentityResult> CreateAdmin()
        {
            try
            {
                var result = _userManager.CreateAsync(new ApplicationUser
                {
                    FirstName = "Erdem",
                    LastName = "Uçar",
                    PhoneNumber = "+905432102644",
                    UserName = "erdemucar87@gmail.com",
                    RegisterDate = DateTime.Now
                }, "er7303032");

                return Ok(result.Result);
            }
            catch
            {
                return BadRequest();
            }

        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromBody] LoginDTO credentials)
        {
            var user = _userManager.FindByNameAsync(credentials.Username);

            // return null if user not found
            if (user == null || user.Result==null)
                return NotFound();

            var checkPassword = _userManager.CheckPasswordAsync(user.Result, credentials.Password);
            if (!checkPassword.Result)
                return NotFound();


            //So we checked, and let's create a valid token with some Claim	         
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = "Soft. Spe. Erdem Ucar",
                Issuer = "aqapplication",
                Subject = new ClaimsIdentity(new Claim[]
                {
                    //Add any claim	       
                    new Claim(ClaimTypes.PrimarySid, user.Result.Id),
                    new Claim(ClaimTypes.Name, credentials.Username),
                    new Claim(ClaimTypes.HomePhone,user.Result.PhoneNumber)
                }),
                //Expire token after some time	                
                Expires = DateTime.UtcNow.AddDays(7),
                //Let's also sign token credentials for a security aspect, this is important!!!	               
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            //So see token info also please check token	       
                return Ok(tokenString);
        }
     }
}