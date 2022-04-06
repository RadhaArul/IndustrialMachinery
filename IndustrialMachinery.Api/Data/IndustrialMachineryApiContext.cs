#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IndustrialMachinery.Core;

namespace IndustrialMachinery.Api.Data
{
    public class IndustrialMachineryApiContext : DbContext
    {
        public IndustrialMachineryApiContext (DbContextOptions<IndustrialMachineryApiContext> options)
            : base(options)
        {
        }

        public DbSet<IndustrialMachinery.Core.Machine> Machine { get; set; }
    }
}
