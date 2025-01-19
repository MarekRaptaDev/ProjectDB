using Microsoft.AspNetCore.Identity;

namespace ProjektDb.Models
{
    public class Users : IdentityUser
    {
        public String FullName { get; set; }
    }
}
