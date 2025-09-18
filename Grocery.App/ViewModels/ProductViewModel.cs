using CommunityToolkit.Mvvm.Messaging;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        public ObservableCollection<Product> Products { get; set; } = new();

        public ProductViewModel(IProductService productService)
        {
            _productService = productService;
            ReloadProducts();
            
            WeakReferenceMessenger.Default.Register<Product>(this, (r, updated) =>
            {
                ReloadProducts();
                
            });
        }
        private void ReloadProducts()
        {
            Products.Clear();
            foreach (var p in _productService.GetAll())
            {
                Products.Add(p);
            }
        }
    }
}
