using AutoAPI.Models.Context;
using AutoAPI.Models.Vehicles.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AutoAPI.Models.Context
{
    public class EContext: DbContext, IContext
    {
        public const string CONNECTION_STRING_NAME = "Auto_EntityFramework";

        private IConfiguration _configuration;

        public EContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
//        public EContext() : base()
//        {
//            
//
//        }
//        public EContext(DbContextOptions<EContext> options) : base(options)
//        {
//            
//        }
        
        public DbSet<Vehicle> Vehicles { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>().ToTable("Vehicle");
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Auto_EntityFramework"));

        }
    }
}