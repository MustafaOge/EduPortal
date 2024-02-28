
using EduPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Persistence.context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Consumer> Consumers { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //{
        //    options.UseNpgsql(connectionString: "User ID=postgres;Password=123456;Host=127.0.0.1;Port=5432;Database=EduPortalDb;");
        //}
    }
}
