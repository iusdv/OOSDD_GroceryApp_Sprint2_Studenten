using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;

namespace Grocery.Core.Data.Repositories
{
    public class GroceryListItemsRepository : IGroceryListItemsRepository
    {
        private readonly List<GroceryListItem> _items;
        private int _nextId;

        public GroceryListItemsRepository()
        {
            _items = new List<GroceryListItem>
            {
                new GroceryListItem(1, 1, 1, 3),
                new GroceryListItem(2, 1, 2, 1),
                new GroceryListItem(3, 1, 3, 4),
                new GroceryListItem(4, 2, 1, 2),
                new GroceryListItem(5, 2, 2, 5),
            };

            _nextId = _items.Max(g => g.Id) + 1;
        }
        public List<GroceryListItem> GetAll() => _items.ToList();

        public GroceryListItem? Get(int id) => _items.FirstOrDefault(i => i.Id == id);

        public GroceryListItem Add(GroceryListItem item)
        {
            if (item.Id <= 0) item.Id = _nextId++;
            _items.Add(item);
            return item;
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            if (item.Id <= 0) throw new ArgumentException("Update needs Id > 0", nameof(item));
            var idx = _items.FindIndex(i => i.Id == item.Id);
            if (idx < 0) return null;
            _items[idx] = item;
            return item;
        }
        public GroceryListItem? Delete(GroceryListItem item)
        {
            var idx = _items.FindIndex(i => i.Id == item.Id);
            if (idx < 0) return null;

            var removed = _items[idx];
            _items.RemoveAt(idx);
            return removed;
        }

        public List<GroceryListItem> GetAllOnGroceryListId(int id)
        {
            return _items.Where(g => g.GroceryListId == id).ToList();
        }
    }
}
