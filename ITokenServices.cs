using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims; // Added this using directive
using System.Text;
using System.Threading.Tasks;

namespace TalabatService
{
   
    public class TokenServices : ITokenServices
    {
        public TokenServices()
        {
        }
        public string CreateToken(User user)
        {
            throw new NotImplementedException();
        }
        public string CreateRefreshToken()
        {
            throw new NotImplementedException();
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            throw new NotImplementedException();
        }
        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
