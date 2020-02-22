using Microsoft.EntityFrameworkCore;
using RemmodPlacesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemodePlacesAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext>options) : base(options)
        {
                
        }
        public DbSet<User> Users { get; set; }

    }
}
