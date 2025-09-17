using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class GroceryListItemsService : IGroceryListItemsService
    {
        private readonly IGroceryListItemsRepository _groceriesRepository;
        private readonly IProductRepository _productRepository;

        public GroceryListItemsService(
            IGroceryListItemsRepository groceriesRepository,
            IProductRepository productRepository)
        {
            _groceriesRepository = groceriesRepository;
            _productRepository = productRepository;
        }

        public List<GroceryListItem> GetAll()
        {
            var list = _groceriesRepository.GetAll();
            FillService(list);
            return list;
        }

        public List<GroceryListItem> GetAllOnGroceryListId(int groceryListId)
        {
            // If your repo has a filtered method, prefer that. Otherwise filter here.
            var list = _groceriesRepository.GetAll()
                                           .Where(g => g.GroceryListId == groceryListId)
                                           .ToList();
            FillService(list);
            return list;
        }

        public GroceryListItem Add(GroceryListItem item)
        {
            var added = _groceriesRepository.Add(item);
            // hydrate Product so callers who show names don’t see “None”
            added.Product = _productRepository.Get(added.ProductId) ?? new(0, "", 0);
            return added;
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            var updated = _groceriesRepository.Update(item);
            if (updated is null) return null;

            updated.Product = _productRepository.Get(updated.ProductId) ?? new(0, "", 0);
            return updated;
        }

        public GroceryListItem? Delete(GroceryListItem item)
        {
            var deleted = _groceriesRepository.Delete(item);
            return deleted;
        }

        public GroceryListItem? Get(int id)
        {
            var one = _groceriesRepository.Get(id);
            if (one is null) return null;

            one.Product = _productRepository.Get(one.ProductId) ?? new(0, "", 0);
            return one;
        }

        private void FillService(List<GroceryListItem> groceryListItems)
        {
            foreach (var g in groceryListItems)
                g.Product = _productRepository.Get(g.ProductId) ?? new(0, "", 0);
        }
    }
}
