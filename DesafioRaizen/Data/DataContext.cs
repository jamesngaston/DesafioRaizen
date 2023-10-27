using DesafioRaizen.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioRaizen.Data
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\LocalDB;" +
                "Database=DesafioRaizen;" +
                "AttachDbFileName=|DataDirectory|DesafioRaizen.mdf;" +
                "Trusted_Connection=True;" +
                "TrustServerCertificate=true;");
        }

        public virtual DbSet<Customer> Customers { get; set; }
    }
}