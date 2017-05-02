namespace WebRole1.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebRole1.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebRole1.DAL.BKCCloudContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebRole1.DAL.BKCCloudContext context)
        {
            List<Customer> customers = new List<Customer>()
            {
                new Customer(){Id=1,FirstName="Bhagyashri",LastName="Chhapwale",Email="bhagchhap@gmail.com"},
                new Customer(){Id=2,FirstName="Abhay",LastName="Bhangale",Email="abhaybhangale@gmail.com"}
            };

            foreach (var cust in customers)
            {
                context.Customers.Add(cust);
            }


            List<Order> orders = new List<Order>()
            {
                new Order() {Id = 1,CustomerId =1,OrderDate=DateTime.Today},
                new Order() {Id = 2,CustomerId =1,OrderDate=DateTime.Today},
                new Order() {Id = 3,CustomerId =2,OrderDate=DateTime.Today},
                new Order() {Id = 4,CustomerId =2,OrderDate=DateTime.Today},
            };

            foreach (var ord in orders)
            {
                context.Orders.Add(ord);
            }
        }
    }
}
