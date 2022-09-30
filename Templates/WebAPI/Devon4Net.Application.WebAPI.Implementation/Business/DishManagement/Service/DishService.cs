using System.Linq.Expressions;
using Devon4Net.Infrastructure.Logger.Logging;
using Devon4Net.Domain.UnitOfWork.Service;
using Devon4Net.Domain.UnitOfWork.UnitOfWork;
using Devon4Net.Application.WebAPI.Implementation.Domain.Database;
using Devon4Net.Application.WebAPI.Implementation.Domain.Entities;
using Devon4Net.Application.WebAPI.Implementation.Domain.RepositoryInterfaces;
using Newtonsoft.Json;
using Devon4Net.Infrastructure.LiteDb.Repository;

namespace Devon4Net.Application.WebAPI.Implementation.Business.DishManagement.Service
{
    /// <summary>
    /// Service implementation
    /// </summary>
    public class DishService : IDishService
    {
        private readonly ILiteDbRepository<Dish> _dishRepository;

        public DishService(ILiteDbRepository<Dish> dishRepository)
        {
            _dishRepository = dishRepository;

            if (!_dishRepository.Get().Any())
            {
                Devon4NetLogger.Debug("No data has been populated. Loading initial data!");

                using (StreamReader file = File.OpenText(@"./LiteDbInitialData.json"))
                {
                    List<Dish> dishes = JsonConvert.DeserializeObject<List<Dish>>(file.ReadToEnd());

                    foreach (var dish in dishes)
                    {
                        _dishRepository.Create(dish);
                    }
                }
            }
        }

        public async Task<List<Dish>> GetDishesMatchingCriterias(decimal maxPrice, int minLikes, string searchBy, IList<long> categoryIdList)
        {
            var result = _dishRepository.Get();

            if (categoryIdList.Any())
            {
                result = result.Where(r => r.Categories.Any(a => categoryIdList.Contains(a.Id))).ToList();
            }

            if (!string.IsNullOrWhiteSpace(searchBy))
            {
                result = result.Where(e => e.Name.Contains(searchBy)).ToList();
            }

            if (maxPrice > 0)
            {
                result = result.Where(e => e.Price < maxPrice).ToList();
            }

            if (minLikes > 0)
            {

            }

            return result.ToList();
        }

        public Task<Dish> GetDishById(long id)
        {
            Devon4NetLogger.Debug($"GetDishById method from service Dishservice with value : {id}");

            var expression = LiteDB.Query.EQ("Id", id);

            var dishWithId = _dishRepository.GetFirstOrDefault(expression);

            return Task.FromResult(dishWithId);
        }
    }
}