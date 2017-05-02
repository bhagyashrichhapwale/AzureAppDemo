using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebRole1.Models;

namespace WebRole1.DAL
{
    public class BKCCloudContext : DbContext
    {
        public BKCCloudContext ()
            : base("BKCCloudContext")
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

    }
}