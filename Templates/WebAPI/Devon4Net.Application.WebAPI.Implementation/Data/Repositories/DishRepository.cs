using System.Linq.Expressions;
using Devon4Net.Domain.UnitOfWork.Repository;
using Devon4Net.Infrastructure.Logger.Logging;
using Devon4Net.Application.WebAPI.Implementation.Domain.Database;
using Devon4Net.Application.WebAPI.Implementation.Domain.Entities;
using Devon4Net.Application.WebAPI.Implementation.Domain.RepositoryInterfaces;
using Newtonsoft.Json;

namespace Devon4Net.Application.WebAPI.Implementation.Data.Repositories
{
    public class DishRepository : Repository<Dish>, IDishRepository
    {
        public DishRepository(
            DishContext context
            ) : base(context)
        {
            if (!context.Dishes.Any())
            {
                Devon4NetLogger.Debug("No data has been populated. Loading initial data!");

                using (StreamReader file = File.OpenText(@"./LiteDbInitialData.json"))
                {
                    List<Dish> dishes = JsonConvert.DeserializeObject<List<Dish>>(file.ReadToEnd());

                    foreach (var dish in dishes)
                    {
                        Create(dish);
                    }
                }
            }
        }

        public Task<Dish> GetDishById(long id)
        {
            Devon4NetLogger.Debug($"GetDishByID method from repository Dishservice with value : {id}");

            return GetFirstOrDefault(t => t.Id == id);
        }

        public async Task<IList<Dish>> GetAllNested(IList<string> nestedProperties, Expression<Func<Dish, bool>> predicate = null)
        {
            return await Get(nestedProperties, predicate);
        }
    }
}