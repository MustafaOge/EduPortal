
using EduPortal.Persistence.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using EduPortal.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using EduPortal.Domain.Entities;

namespace EduPortal.Persistence.context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Individual> Individuals { get; set; }
        public DbSet<Corporate> Corprorates { get; set; }
        //public DbSet<Subscriber> Subscribers { get; set; }


        //public DbSet<Book> Books { get; set; }

        //public DbSet<Consumer> Consumers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=EduPortal; Trusted_Connection=true;TrustServerCertificate=True;");
                optionsBuilder.UseLazyLoadingProxies(false);
            }
            base.OnConfiguring(optionsBuilder);
        }


    }

}
