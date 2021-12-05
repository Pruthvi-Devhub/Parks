using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Models
{
    public class ApplicationDbContext:DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> Options) :base(Options)
        {

        }

        public DbSet<NationalPark> NationalParks { get; set; }
        //public object Trails { get; internal set; }
        public DbSet<Trail> Trails { get; set; }
    }
}
