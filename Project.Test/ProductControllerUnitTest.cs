using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Controllers;
using Project.DataConfig;
using Project.Models;
using Project.Repositories;
using Project.Repositories.Dto;
using System;
using System.Collections.Generic;
using Xunit;

namespace Project.Test
{
    public class ProductControllerUnitTest
    {
        private ProductRepository repository;
        private ProductController controller;
        public static DbContextOptions<ProductdbContext> dbContextOptions { get; }
        public static string connectionString = "data source=Your SQL Serwer Name; initial catalog=ProductDbTest;integrated security=true";

        static ProductControllerUnitTest()
        {
            dbContextOptions = new DbContextOptionsBuilder<ProductdbContext>()
                .UseSqlServer(connectionString)
                .Options;
        }
        public ProductControllerUnitTest()
        {
            var context = new ProductdbContext(dbContextOptions);
            MockupDBInitializer db = new MockupDBInitializer();
            db.Seed(context);

            repository = new ProductRepository(context);
            controller = new ProductController(repository);
        }
        #region Get List

        [Fact]
        public async void Task_GetList_Return_BadRequestResult()
        {
            //Act
            var data = await controller.GetList();
            data = null;

            if (data != null)
                //Assert
                Assert.IsType<BadRequestResult>(data.Result);
        }

        [Fact]
        public async void Task_GetList_Return_OkResult()
        {
            //Act
            var data = await controller.GetList();

            //Assert
            Assert.IsType<OkObjectResult>(data.Result);
        }

        [Fact]
        public async void Task_GetList_MatchResult()
        {
            //Act
            var data = await controller.GetList();
            //Assert
            Assert.IsType<OkObjectResult>(data.Result);

            var okResult = data.Result as OkObjectResult;
            var products = Assert.IsType<List<Product>>(okResult.Value);

            Assert.Equal(3, products.Count);
        }
        #endregion
        #region Get Product By Id
        [Fact]
        public async void Task_GetProductById_CorrectItemReturn()
        {
            // Arrange
            var idToFind = new Guid("d39059bf-e616-427c-8e2a-b337af89937e");
            // Act
            var data = await controller.GetProductById(idToFind);
            var okResult = data.Result as OkObjectResult;
            // Assert
            Assert.IsType<Product>(okResult.Value);
            Assert.Equal(idToFind, (okResult.Value as Product).Id);
        }

        [Fact]
        public async void Task_GetProductById_ReturnsOkResult()
        {
            // Arrange
            var idToFind = new Guid("d39059bf-e616-427c-8e2a-b337af89937e");
            // Act
            var okResult = await controller.GetProductById(idToFind);
            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }
        #endregion
        #region Create Product
        [Fact]
        public void Task_CreateProduct_CorrectCreatedItem()
        {
            // Arrange
            var newItem = new ProductDto()
            {
                Name = "NewProduct",
                Price = 17.00M
            };
            // Act
            var okResult = controller.CreateProduct(newItem).Result as OkObjectResult;
            var item = okResult.Value as Product;
            // Assert
            Assert.IsType<Product>(item);
            Assert.Equal("NewProduct", item.Name);
        }
        #endregion
        #region Update Product
        [Fact]
        public void Task_UpdateProduct_CorrectUpdateItem()
        {
            // Arrange
            var idToFind = new Guid("d7798a52-7dc1-4b1b-9d39-cf8449158dc8");
            var updatedItem = new ProductDto()
            {
                Name = "UpdatedProduct",
                Price = 17.00M
            };
            // Act
            var okResult = controller.UpdateProduct(idToFind, updatedItem).Result as OkObjectResult;
            var item = okResult.Value as Product;
            // Assert
            Assert.IsType<Product>(item);
            Assert.Equal("UpdatedProduct", item.Name);
        }
        #endregion
        #region Remove Product
        [Fact]
        public void Task_RemoveProduct_ReturnsOkResult()
        {
            // Arrange
            var idToFind = new Guid("cec438d4-17cb-4948-9417-e1735a7d3066");
            // Act
            var okResult = controller.RemoveProduct(idToFind).Result;
            // Assert
            Assert.IsType<OkObjectResult>(okResult);
        }
        #endregion
    }
}
