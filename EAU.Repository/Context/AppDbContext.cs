using EAU.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAU.Repository.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Consumer> Consumers { get; set; }  
        public DbSet<Invoice> Invoices { get; set; }    
        public DbSet<Plumbing> Plumbings { get; set; }  
        public DbSet<Subscriber> Subscribers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(connectionString: "User ID=postgres;Password=123456;Host=127.0.0.1;Port=5432;Database=EAUDB;");
        }


    }



}
