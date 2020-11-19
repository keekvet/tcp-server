using MessageCore.Models;
using Microsoft.EntityFrameworkCore;


namespace tcp_server
{
    class ApplicationContext : DbContext
    {
        private static ApplicationContext context;

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        private ApplicationContext()
        {
            Database.EnsureCreated();
        }

        public static ApplicationContext getContext()
        {
            if (context is null)
                context = new ApplicationContext();
            return context;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.EnableSensitiveDataLogging(true);
                
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Message>()
                //.HasOne<User>("Sender")
                //.WithMany()
                //.OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Message>()
                //.HasOne<User>("Receiver")
                //.WithMany()
                //.OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
