using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mesa03.Models
{
    public class Mesa03Context : DbContext
    {
        public Mesa03Context (DbContextOptions<Mesa03Context> options)
            : base(options)
        {
        }

        public DbSet<Mesa03.Models.Department> Department { get; set; }
    }
}
