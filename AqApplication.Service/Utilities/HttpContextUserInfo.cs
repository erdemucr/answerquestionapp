using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace AqApplication.Service.Utilities
{
    public static class HttpContextUserInfo
    {
        public static string GetUserId(IIdentity _identity)
        {
            try
            {
                var identity = (ClaimsIdentity)_identity;
                IEnumerable<Claim> claims = identity.Claims;
                foreach (var item in claims)
                {
                    if (item.Type ==ClaimTypes.PrimarySid)
                        return item.Value;
                }
                throw new Exception();
            }
            catch
            {

                throw ;
            }
        }
    }
}
