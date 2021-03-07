using Project.DataConfig;
using Project.Models;
using System;

namespace Project.Test
{
    public class MockupDBInitializer
    {
        public void Seed(ProductdbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Products.AddRange(
                new Product() { Id = new Guid("d39059bf-e616-427c-8e2a-b337af89937e"), Name = "Product1", Price = 10.00M },
                new Product() { Id = new Guid("d7798a52-7dc1-4b1b-9d39-cf8449158dc8"), Name = "Product2", Price = 12.34M },
                new Product() { Id = new Guid("cec438d4-17cb-4948-9417-e1735a7d3066"), Name = "Product3", Price = 77.77M }
            );

            context.SaveChanges();
        }
    }
}
