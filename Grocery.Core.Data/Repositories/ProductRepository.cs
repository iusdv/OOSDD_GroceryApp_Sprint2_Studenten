// Grocery.Core.Data.Repositories/ProductRepository.cs
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;

namespace Grocery.Core.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _items;
        private int _nextId;

        public ProductRepository()
        {
            _items = new List<Product>
            {
                new Product(1, "Melk", 300),
                new Product(2, "Kaas", 20),
                new Product(3, "Brood", 400),
                new Product(4, "Cornflakes", 2)
            };

            _nextId = _items.Max(p => p.Id) + 1;
        }
        public List<Product> GetAll() => _items.ToList();
        public Product? Get(int id) => _items.FirstOrDefault(p => p.Id == id);
        public Product Add(Product product)
        {
            if (product.Id <= 0) product.Id = _nextId++;
            _items.Add(product);
            return product;
        }
        public Product? Update(Product product)
        {
            if (product.Id <= 0)
                throw new ArgumentException("Update requires Id > 0", nameof(product));

            var existing = _items.FirstOrDefault(p => p.Id == product.Id);
            if (existing is null) return null;
            existing.Name  = product.Name;
            existing.Stock = product.Stock;

            return existing;
        }
        public Product? Delete(Product product)
        {
            var idx = _items.FindIndex(p => p.Id == product.Id);
            if (idx < 0) return null;

            var removed = _items[idx];
            _items.RemoveAt(idx);
            return removed;
        }
    }
}
