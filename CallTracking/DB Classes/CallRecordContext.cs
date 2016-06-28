using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CallTracking.DB_Classes
{

    class CallRecordContext : DbContext
    {

        public CallRecordContext()
            : base("DefaultConnection")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<Bank> Banks { get; set; }
        public DbSet<Config> Configs { get; set; } 
        public DbSet<Record> Records { get; set; }


    }
}
