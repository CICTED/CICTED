using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CICTED.Domain.Infrastucture.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>        
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
         

    }
}
