using Microsoft.AspNetCore.Identity;

namespace Crypto.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
