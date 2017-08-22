using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CICTED.Domain.Entities.Account;
using Microsoft.AspNetCore.Identity;

namespace CICTED.Domain.Infrastucture.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Roles, long>        
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
    }
}
