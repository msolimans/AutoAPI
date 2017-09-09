using AutoAPI.Models.Context;
using AutoAPI.Models.Vehicles.Model;
using Microsoft.EntityFrameworkCore;

namespace AutoAPI.Models.Context
{
    public class EContext: DbContext, IContext
    {
        public const string CONNECTION_STRING_NAME = "Auto_EntityFramework";

        public EContext() : base()
        {
            
        }
        public EContext(DbContextOptions<EContext> options) : base(options)
        {
            
        }
        
        public DbSet<Vehicle> Vehicles { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>().ToTable("Vehicle");
        }
        
    }
}