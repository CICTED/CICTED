using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CICTED.Domain.Entities.Account
{
    public class Roles : IdentityRole<long>
    {
        public Roles() :base()
        {

        }

        public Roles(string roleName)
        {
            Name = roleName;
        }
    }
}
