using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using drevolution.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace drevolution.data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser> {

    public ApplicationDbContext(DbContextOptions options)  : base(options)
    {
    }

    public DbSet<drevolution.Models.Entities.AppUser> AppUser { get; set; }
    

  }

}
