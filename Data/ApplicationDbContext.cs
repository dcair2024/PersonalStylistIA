using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalStylistIA.Models;

namespace PersonalStylistIA.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
        {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
    
}
