using DeRoso.Core.Health;
using Microsoft.EntityFrameworkCore;
using System;

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


        /// <summary>
        /// Таблица рецептов
        /// </summary>
        public DbSet<HealthTestReciept> Reciepts { get; set; }

        public DbSet<HealthTestSelected> LastSelected { get; set; }

        /// <summary>
        /// Таблица пциентов
        /// </summary>
        public DbSet<Patient> Patients { get; set; }


        public DeRosoContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();  
           
        }

        /// <summary>
        /// Загрузка таблиц
        /// </summary>
        public void Load()
        {
            try
            {
                Drugs.Load();
                Tests.Load();
                Groups.Load();
                Sections.Load();
                Patients.Load();
                Reciepts.Load();
                LastSelected.Load();
            }
            catch(Exception e)
            {
                
            }
        }

        public DeRosoContext(DbContextOptions<DeRosoContext> options, bool recreate)
            : base(options)
        {
            if (recreate)
                Database.EnsureDeleted();

            Database.EnsureCreated();
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
