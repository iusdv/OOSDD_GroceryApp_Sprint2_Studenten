using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository) => _productRepository = productRepository;
        
        public List<Product> GetAll() =>  _productRepository.GetAll();
        public Product Add(Product item) => _productRepository.Add(item);
        
        public Product? Delete(Product item) =>  _productRepository.Delete(item);
        
        public Product? Get(int id) => _productRepository.Get(id);
        
        public Product? Update(Product item) =>  _productRepository.Update(item);
        
    }
}
