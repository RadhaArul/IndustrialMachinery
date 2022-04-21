#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Machinary.Api;

namespace Machinary.Api.Data
{
    public class MachinaryApiContext : DbContext
    {
        public MachinaryApiContext (DbContextOptions<MachinaryApiContext> options)
            : base(options)
        {
        }
      
        public DbSet<Machinary.Api.Machine> Machine { get; set; }
    }
}
