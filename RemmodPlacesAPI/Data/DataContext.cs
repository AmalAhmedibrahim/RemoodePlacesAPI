using Microsoft.EntityFrameworkCore;
using RemodePlacesAPI.Models;
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
      public  DbSet<Reviews> Reviews { get; set;  }
    }
}
