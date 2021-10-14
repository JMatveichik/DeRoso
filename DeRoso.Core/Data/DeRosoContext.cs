using DeRoso.Core.Health;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Data
{
   
    public class DeRosoContext : DbContext
    {
        /// <summary>
        /// Таблица разделов тестов
        /// </summary>
        public DbSet<HealthTestSection> Sections { get; set; }

        /// <summary>
        /// Таблица групп тестов
        /// </summary>
        public DbSet<HealthTestGroup> Groups { get; set; }

        /// <summary>
        /// Таблица препаратов
        /// </summary>
        public DbSet<HealthTestDrug> Drugs { get; set; }

        /// <summary>
        /// Таблица тестов
        /// </summary>
        public DbSet<HealthTest> Tests { get; set; }


        public DeRosoContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();            
        }

        public DeRosoContext(DbContextOptions<DeRosoContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Filename=DeRoso.db");
            }
        }
    }
}
