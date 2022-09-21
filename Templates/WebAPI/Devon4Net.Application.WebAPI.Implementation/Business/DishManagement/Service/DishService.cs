using System.Linq.Expressions;
using Devon4Net.Infrastructure.Logger.Logging;
using Devon4Net.Domain.UnitOfWork.Service;
using Devon4Net.Domain.UnitOfWork.UnitOfWork;
using Devon4Net.Application.WebAPI.Implementation.Domain.Database;
using Devon4Net.Application.WebAPI.Implementation.Domain.Entities;
using Devon4Net.Application.WebAPI.Implementation.Domain.RepositoryInterfaces;
using Newtonsoft.Json;

namespace Devon4Net.Application.WebAPI.Implementation.Business.DishManagement.Service
{
    /// <summary>
    /// Service implementation
    /// </summary>
    public class DishService : Service<DishContext>, IDishService
    {
        private readonly IDishRepository _dishRepository;

        public DishService(IUnitOfWork<DishContext> uoW) : base(uoW)
        {
            _dishRepository = uoW.Repository<IDishRepository>();
        }

        public async Task<List<Dish>> GetDishesMatchingCriterias(decimal maxPrice, int minLikes, string searchBy, IList<long> categoryIdList)
        {
            var result = await _dishRepository.Get().ConfigureAwait(false);

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

            return _dishRepository.GetDishById(id);
        }
    }
}