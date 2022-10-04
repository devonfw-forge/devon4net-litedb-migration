using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using Devon4Net.Application.WebAPI.Implementation.Business.DishManagement.Service;
using System.Threading.Tasks;
using Devon4Net.Application.WebAPI.Implementation.Domain.Entities;
using System.Linq;
using Devon4Net.Application.WebAPI.Implementation.Business.DishManagement.Controllers;
using Devon4Net.Application.WebAPI.Implementation.Business.DishManagement.Dto;
using Devon4Net.Application.WebAPI.Implementation.Business.DishManagement.Converters;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace Devon4Net.Test.xUnit.Test.UnitTest.Management.Controllers
{
    public class DishControllerTests : UnitTest
    {
        private readonly Random rand = new();
        [Fact]
        public async Task DishSearch_WithGivenCategoryFilterCriteria_ReturnsAllCorrespondingDishes()
        {
            //Arrange
            var dishes = new[] { CreateRandomDish(), CreateRandomDish(), CreateRandomDish() };
            var randomPrice = (decimal)rand.NextDouble();
            var minLikes = rand.Next();
            var searchBy = Guid.NewGuid().ToString();
            var expectedCategories = new[] { dishes[0].Categories, dishes[1].Categories };
            IList<long> categoryObjIds = new List<long>() { expectedCategories[0].FirstOrDefault().Id, expectedCategories[1].FirstOrDefault().Id };

            var categorySearchDtoIds = categoryObjIds.Select(c => new CategorySearchDto
            {
                Id = c,
            }).ToList().ToArray();

            List<Dish> expectedDishes = dishes.Where(dish => dish.Categories.Any(cat => categoryObjIds.Contains(cat.Id))).ToList();

            var serviceStub = new Mock<IDishService>();
            serviceStub.Setup(repo => repo.GetDishesMatchingCriterias(
                    //It.IsAny<decimal>(),
                    It.IsAny<decimal>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.Is<IList<long>>(categoryIdList => categoryIdList.All(cat => categoryObjIds.Contains(cat)))
                    )).Returns(Task.FromResult(expectedDishes));

            var controller = new DishController(serviceStub.Object);
            //Act
            var filterDto = new FilterDtoSearchObjectDto { MaxPrice = randomPrice, SearchBy = searchBy, MinLikes = minLikes, Categories = categorySearchDtoIds };
            var expectedResult = new ResultObjectDto<DishDtoResult> { };
            expectedResult.content = expectedDishes.Select(DishConverter.EntityToApi).ToList();
            expectedResult.Pagination.Total = expectedDishes.Count();

            var result = (ObjectResult)await controller.DishSearch(filterDto);

            //Assert
            var compareObj = JsonConvert.SerializeObject(expectedResult);
            Assert.Equal(compareObj, result.Value);
        }

        [Fact]
        public async Task DishSearch_WithGivenMaxPriceFilterCriteria_ReturnsAllCorrespondingDishes()
        {
            //Arrange
            var Dishes = new[] { CreateRandomDish(), CreateRandomDish(), CreateRandomDish() };
            var DishPrices = new[] { Dishes[0].Price, Dishes[1].Price, Dishes[2].Price };

            //Price Filter is set to max. Therefore all dishes will be taken into account.
            var expectedPrice = DishPrices.Max();
            var minLikes = rand.Next();
            var searchBy = Guid.NewGuid().ToString();
            var expectedCategories = Array.Empty<ICollection<Category>>();
            IList<long> categoryObjIds = new List<long>() { };
            var categorySearchDtoIds = categoryObjIds.Select(c => new CategorySearchDto
            {
                Id = c,
            }).ToList().ToArray();

            List<Dish> expectedDishes = Dishes.Where(dish => dish.Price <= expectedPrice).ToList();

            var serviceStub = new Mock<IDishService>();
            serviceStub.Setup(repo => repo.GetDishesMatchingCriterias(
                    //It.IsAny<decimal>(),
                    It.Is<decimal>(price => price == expectedPrice),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<IList<long>>())).Returns(Task.FromResult(expectedDishes));

            var controller = new DishController(serviceStub.Object);
            //Act
            var filterDto = new FilterDtoSearchObjectDto { MaxPrice = expectedPrice, SearchBy = searchBy, MinLikes = minLikes, Categories = categorySearchDtoIds };
            var expectedResult = new ResultObjectDto<DishDtoResult> { };
            expectedResult.content = expectedDishes.Select(DishConverter.EntityToApi).ToList();
            expectedResult.Pagination.Total = expectedDishes.Count();

            var result = (ObjectResult)await controller.DishSearch(filterDto);

            //Assert
            var compareObj = JsonConvert.SerializeObject(expectedResult);
            Assert.Equal(compareObj, result.Value);
        }

        [Fact]
        public async Task DishSearch_WithGivenMinPriceFilterCriteria_ReturnsAllCorrespondingDishes()
        {
            //Arrange
            var dishes = new[] { CreateRandomDish(), CreateRandomDish(), CreateRandomDish() };
            var dishPrices = new[] { dishes[0].Price, dishes[1].Price, dishes[2].Price };

            //Price Filter is set to min. Therefore only the cheapest dish(es) will be taken into account.
            var expectedPrice = dishPrices.Min();
            var minLikes = rand.Next();
            var searchBy = Guid.NewGuid().ToString();
            var expectedCategories = Array.Empty<ICollection<Category>>();
            IList<long> categoryObjIds = new List<long>() { };
            var categorySearchDtoIds = categoryObjIds.Select(c => new CategorySearchDto
            {
                Id = c,
            }).ToList().ToArray();

            List<Dish> expectedDishes = dishes.Where(dish => dish.Price <= expectedPrice).ToList();

            var serviceStub = new Mock<IDishService>();
            serviceStub.Setup(repo => repo.GetDishesMatchingCriterias(
                    //It.IsAny<decimal>(),
                    It.Is<decimal>(price => price == expectedPrice),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<IList<long>>())).Returns(Task.FromResult(expectedDishes));

            var controller = new DishController(serviceStub.Object);
            //Act
            var filterDto = new FilterDtoSearchObjectDto { MaxPrice = expectedPrice, SearchBy = searchBy, MinLikes = minLikes, Categories = categorySearchDtoIds };
            var expectedResult = new ResultObjectDto<DishDtoResult> { };
            expectedResult.content = expectedDishes.Select(DishConverter.EntityToApi).ToList();
            expectedResult.Pagination.Total = expectedDishes.Count();

            var result = (ObjectResult)await controller.DishSearch(filterDto);

            //Assert
            var compareObj = JsonConvert.SerializeObject(expectedResult);
            Assert.Equal(compareObj, result.Value);
        }

        [Fact]
        public async Task DishSearch_WithGivenSearchByCriteria_ReturnsAllCorrespondingDishes()
        {
            //Arrange
            var dishes = new[] { CreateRandomDish(), CreateRandomDish(), CreateRandomDish() };
            var expectedPrice = (decimal)rand.NextDouble();
            var minLikes = rand.Next();
            //Choose randomly one of the Dish names to filter for
            var dishNames = new[] { dishes[0].Name, dishes[1].Name, dishes[2].Name };
            var searchBy = dishNames[rand.Next(0, 3)];

            var expectedCategories = Array.Empty<ICollection<Category>>();
            IList<long> categoryObjIds = new List<long>() { };
            var categorySearchDtoIds = categoryObjIds.Select(c => new CategorySearchDto
            {
                Id = c,
            }).ToList().ToArray();

            List<Dish> expectedDishes = dishes.Where(dish => dish.Name.Contains(searchBy, StringComparison.CurrentCultureIgnoreCase)).ToList();

            var serviceStub = new Mock<IDishService>();
            serviceStub.Setup(repo => repo.GetDishesMatchingCriterias(
                    It.IsAny<decimal>(),
                    It.IsAny<int>(),
                    It.Is<string>(name => name == searchBy),
                    It.IsAny<IList<long>>())).Returns(Task.FromResult(expectedDishes));

            var controller = new DishController(serviceStub.Object);
            //Act
            var filterDto = new FilterDtoSearchObjectDto { MaxPrice = expectedPrice, SearchBy = searchBy, MinLikes = minLikes, Categories = categorySearchDtoIds };
            var expectedResult = new ResultObjectDto<DishDtoResult> { };
            expectedResult.content = expectedDishes.Select(DishConverter.EntityToApi).ToList();
            expectedResult.Pagination.Total = expectedDishes.Count();

            var result = (ObjectResult)await controller.DishSearch(filterDto);

            //Assert
            var compareObj = JsonConvert.SerializeObject(expectedResult);
            Assert.Equal(compareObj, result.Value);
        }

        [Fact]
        public async Task DishSearch_WithFullFilterCriteria_ReturnsAllCorrespondingDishes()
        {
            //Arrange
            var dishes = new[] { CreateRandomDish(), CreateRandomDish(), CreateRandomDish() };
            var dishPrices = new[] { dishes[0].Price, dishes[1].Price, dishes[2].Price };

            //Price Filter is set to max. Therefore all dishes will be taken into account.
            var expectedPrice = dishPrices.Max();
            var minLikes = rand.Next();

            //Choose randomly one of the Dish names to filter for
            var dishNames = new[] { dishes[0].Name, dishes[1].Name, dishes[2].Name };
            var searchBy = dishNames[rand.Next(0, 3)];

            var expectedCategories = new[] { dishes[0].Categories, dishes[1].Categories };

            IList<long> categoryObjIds = new List<long>() { expectedCategories[0].FirstOrDefault().Id, expectedCategories[1].FirstOrDefault().Id };
            var categorySearchDtoIds = categoryObjIds.Select(c => new CategorySearchDto
            {
                Id = c,
            }).ToList().ToArray();

            List<Dish> expectedDishes = dishes.Where(dish =>
                dish.Categories.Any(cat => categoryObjIds.Contains(cat.Id)) &&
                dish.Price <= expectedPrice &&
                dish.Name.Contains(searchBy, StringComparison.CurrentCultureIgnoreCase)).ToList();

            var serviceStub = new Mock<IDishService>();
            serviceStub.Setup(repo => repo.GetDishesMatchingCriterias(
                    //It.IsAny<decimal>(),
                    It.Is<decimal>(price => price == expectedPrice),
                    It.Is<int>(likes => likes == minLikes),
                    It.Is<string>(searchCriteria => searchCriteria == searchBy),
                    It.Is<IList<long>>(categoryIdList => categoryIdList.All(cat => categoryObjIds.Contains(cat)))
                    )).Returns(Task.FromResult(expectedDishes));

            var controller = new DishController(serviceStub.Object);
            //Act

            var filterDto = new FilterDtoSearchObjectDto { MaxPrice = expectedPrice, SearchBy = searchBy, MinLikes = minLikes, Categories = categorySearchDtoIds };
            var expectedResult = new ResultObjectDto<DishDtoResult> { };
            expectedResult.content = expectedDishes.Select(DishConverter.EntityToApi).ToList();
            expectedResult.Pagination.Total = expectedDishes.Count();

            var result = (ObjectResult)await controller.DishSearch(filterDto);

            //Assert
            var compareObj = JsonConvert.SerializeObject(expectedResult);
            Assert.Equal(compareObj, result.Value);
        }

        [Fact]
        public async Task DishSearch_WithNullFilterCriteria_ReturnsDefault()
        {
            //Arrange
            List<Dish> Dishes = new List<Dish> { CreateRandomDish(), CreateRandomDish(), CreateRandomDish() };
            var serviceStub = new Mock<IDishService>();

            serviceStub.Setup(repo => repo.GetDishesMatchingCriterias(
                    It.IsAny<decimal>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<IList<long>>())).Returns(Task.FromResult(Dishes));

            var controller = new DishController(serviceStub.Object);
            //Act
            var expectedResult = new ResultObjectDto<DishDtoResult> { };
            expectedResult.content = Dishes.Select(DishConverter.EntityToApi).ToList();
            expectedResult.Pagination.Total = Dishes.Count();

            var result = (ObjectResult)await controller.DishSearch(null);

            //Assert
            var compareObj = JsonConvert.SerializeObject(expectedResult);
            Assert.Equal(compareObj, result.Value);
        }

        private Image CreateRandomImage()
        {
            return new()
            {
                Content = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                MimeType = Guid.NewGuid().ToString(),
                Extension = Guid.NewGuid().ToString(),
                ContentType = rand.Next(),
            };
        }

        private Category CreateRandomCategory()
        {
            return new()
            {
                Id = rand.NextInt64(),
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
            };
        }

        private Dish CreateRandomDish()
        {
            return new()
            {
                Id = rand.NextInt64(),
                Name = Guid.NewGuid().ToString(),
                Price = ((decimal)rand.NextDouble() + rand.Next()),
                Image = CreateRandomImage(),
                Categories = new[] { CreateRandomCategory(), CreateRandomCategory(), CreateRandomCategory() }
            };
        }
    }
}
