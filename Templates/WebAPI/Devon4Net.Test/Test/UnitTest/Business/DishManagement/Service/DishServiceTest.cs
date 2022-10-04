using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using Devon4Net.Application.WebAPI.Implementation.Business.DishManagement.Service;
using Devon4Net.Application.WebAPI.Implementation.Domain.Database;
using Devon4Net.Application.WebAPI.Implementation.Domain.Entities;
using Devon4Net.Application.WebAPI.Implementation.Domain.RepositoryInterfaces;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Devon4Net.Infrastructure.LiteDb.Repository;

namespace Devon4Net.Test.xUnit.Test.UnitTest.Business
{
    public class DishServiceTest : UnitTest
    {
        public static IList<Dish> buildListOfExampleDishes()
        {
            IList<Dish> dishList = new List<Dish>();

            Category category1 = new Category();
            category1.Id = 1;
            Category category2 = new Category();
            category2.Id = 2;

            Dish dish1 = new Dish();
            dish1.Id = 1;
            dish1.Name = "falafel";
            dish1.Price = 6;
            dish1.Categories.Add(category1);

            Dish dish2 = new Dish();
            dish2.Id = 2;
            dish2.Name = "schnitzel";
            dish2.Price = 8;
            dish2.Categories.Add(category2);

            Dish dish3 = new Dish();
            dish3.Id = 3;
            dish3.Name = "salad";
            dish3.Price = 5;
            dish3.Categories.Add(category1);
            dish3.Categories.Add(category2);

            dishList.Add(dish1);
            dishList.Add(dish2);
            dishList.Add(dish3);

            return dishList;
        }

        [Fact]
        public async void getDishesMatchingCriterias_Returns_Price_Less_Than_6()
        {
            //Arrange
            IList<Dish> dishList = buildListOfExampleDishes();

            var dishRepositoryMock = new Mock<ILiteDbRepository<Dish>>();

            dishRepositoryMock.Setup(
                repository => repository.Get()).Returns(
                    dishList
                );
            
            DishService dishService = new DishService(dishRepositoryMock.Object);
            decimal maxPrice = 6;
            int minLikes = 0;
            string searchBy = "";
            IList<long> categoryIdList = new List<long>();

            var expectedList = new List<Dish>();
            expectedList.Add(dishList[2]);
            var expectedResult = await Task.FromResult(expectedList);
            //Act
            var result = await dishService.GetDishesMatchingCriterias(maxPrice, minLikes, searchBy, categoryIdList);

            Console.WriteLine(result);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async void getDishesMatchingCriterias_Where_CategoryId_Is_1()
        {
            //Arrange
            IList<Dish> dishList = buildListOfExampleDishes();


            var dishRepositoryMock = new Mock<ILiteDbRepository<Dish>>();

            dishRepositoryMock.Setup(
                repository => repository.Get()).Returns(
                    dishList
                );
            
            DishService dishService = new DishService(dishRepositoryMock.Object);

            decimal maxPrice = 0;
            int minLikes = 0;
            string searchBy = "";
            IList<long> categoryIdList = new List<long>();
            categoryIdList.Add(1);

            var expectedList = new List<Dish>();
            expectedList.Add(dishList[0]);
            expectedList.Add(dishList[2]);
            var expectedResult = await Task.FromResult(expectedList);
            //Act
            var result = await dishService.GetDishesMatchingCriterias(maxPrice, minLikes, searchBy, categoryIdList);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async void getDishesMatchingCriterias_SearchBy()
        {
            //Arrange
            IList<Dish> dishList = buildListOfExampleDishes();


            var dishRepositoryMock = new Mock<ILiteDbRepository<Dish>>();

            dishRepositoryMock.Setup(
                repository => repository.Get()).Returns(
                    dishList
                );
            
            DishService dishService = new DishService(dishRepositoryMock.Object);

            decimal maxPrice = 0;
            int minLikes = 0;
            string searchBy = "salad";
            IList<long> categoryIdList = new List<long>();


            var expectedList = new List<Dish>();
            expectedList.Add(dishList[2]);

            var expectedResult = await Task.FromResult(expectedList);
            //Act
            var result = await dishService.GetDishesMatchingCriterias(maxPrice, minLikes, searchBy, categoryIdList);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async void getDishByIdTest()
        {
            //Arrange
            long queriedId = 1;

            Dish dish = new Dish();
            dish.Id = queriedId;
            dish.Name = "falafel";
            dish.Price = 6;

            var dishRepositoryMock = new Mock<ILiteDbRepository<Dish>>();

            dishRepositoryMock.Setup(repository => repository.GetFirstOrDefault(It.IsAny<LiteDB.BsonExpression>())).Returns(dish);
            
            DishService dishService = new DishService(dishRepositoryMock.Object);

            //Act
            var result = await dishService.GetDishById(queriedId);

            //Assert
            dishRepositoryMock.Verify(s => s.GetFirstOrDefault(
                    It.IsAny<LiteDB.BsonExpression>()
                    ),
                    Times.Once());

            Assert.Equal(dish, result);
        }
    }
}
