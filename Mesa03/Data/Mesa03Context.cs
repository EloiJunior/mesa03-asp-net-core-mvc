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

        public DbSet<Department> Department { get; set; }
        public DbSet<Seller> Seller { get; set; }            //pra que o Entity Framework reconheça a classe criada, eu preciso adicionar a classe como o DBSet
        public DbSet<SalesRecord> SalesRecord { get; set; }

    }
}
