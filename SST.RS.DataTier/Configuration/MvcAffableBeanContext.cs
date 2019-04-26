using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SST.RS.Common.BusinessObjects;

namespace SSTDbContext
{
    public class StoreDbContext:DbContext

    {
        public StoreDbContext() : base("ConnString")
        {

        }
       
        public DbSet<AppUsers> Categories { get; set; }

        //public DbSet<Customer> Customers { get; set; }

        //public DbSet<Product> Products { get; set; }

        //public DbSet<CustomerOrder> CustomerOrders { get; set; }

        //public DbSet<OrderedProduct> Orderedproducts { get; set; }

        //public DbSet<Cart> Carts { get; set; }
        //public DbSet<OrderDetail> OrderDetail { get; set; }

    }
}