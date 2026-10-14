using ElAmal.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ElAmal.Infrastructure.Identity.Context
{
    public class ElAmalIdentityDbContext  : IdentityDbContext<ApplicationUser>
    {
        public ElAmalIdentityDbContext(DbContextOptions<ElAmalIdentityDbContext> options)
            : base(options)
        {
            
        }
    }
}
