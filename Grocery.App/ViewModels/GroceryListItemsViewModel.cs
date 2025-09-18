using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.Messaging;

namespace Grocery.App.ViewModels
{
    [QueryProperty(nameof(GroceryList), nameof(GroceryList))]
    public partial class GroceryListItemsViewModel : BaseViewModel
    {
        private readonly IGroceryListItemsService _groceryListItemsService;
        private readonly IProductService _productService;
        public ObservableCollection<GroceryListItem> MyGroceryListItems { get; set; } = [];
        public ObservableCollection<Product> AvailableProducts { get; set; } = [];

        [ObservableProperty] GroceryList groceryList = new(0, "None", DateOnly.MinValue, "", 0);

        public GroceryListItemsViewModel(IGroceryListItemsService groceryListItemsService,
            IProductService productService)
        {
            _groceryListItemsService = groceryListItemsService;
            _productService = productService;
            Load(groceryList.Id);
        }
        private void RemoveAvailableById(int productId)
        {
            for (int i = 0; i < AvailableProducts.Count; i++)
            {
                if (AvailableProducts[i].Id == productId)
                {
                    AvailableProducts.RemoveAt(i);
                    break;
                }
            }
        }
        private void RefreshAvailableProduct(Product product)
        {
            for (int i = 0; i < AvailableProducts.Count; i++)
            {
                if (AvailableProducts[i].Id == product.Id)
                {
                    var idx = i;
                    var item = AvailableProducts[i];
                    AvailableProducts.RemoveAt(idx);
                    AvailableProducts.Insert(idx, item);
                    break;
                }
            }
        }
        private void Load(int id)
        {
            MyGroceryListItems.Clear();
            foreach (var item in _groceryListItemsService.GetAllOnGroceryListId(id))
            {
                if (item.Product == null)
                    item.Product = _productService.GetAll().FirstOrDefault(p => p.Id == item.ProductId);
                MyGroceryListItems.Add(item);
            }
            GetAvailableProducts();
        }
        private void GetAvailableProducts()
        {
            //Maak de lijst AvailableProducts leeg
            //Haal de lijst met producten op
            //Controleer of het product al op de boodschappenlijst staat, zo niet zet het in de AvailableProducts lijst
            //Houdt rekening met de voorraad (als die nul is kun je het niet meer aanbieden).

            AvailableProducts.Clear();
            var allProducts = _productService.GetAll();

            var onList = MyGroceryListItems
                .Select(item => item.ProductId)
                .ToHashSet();

            foreach (var product in allProducts)
            {
                if (product is null) continue;
                if (product.Id <= 0) continue;
                if (product.Stock <= 0) continue;
                AvailableProducts.Add(product);
            }
        }
        partial void OnGroceryListChanged(GroceryList value)
        {
            Load(value.Id);
        }

        [RelayCommand]
        public async Task ChangeColor()

        {
            Dictionary<string, object> paramater = new() { { nameof(GroceryList), GroceryList } };
            await Shell.Current.GoToAsync($"{nameof(ChangeColorView)}?Name={GroceryList.Name}", true, paramater);
        }
        
        private bool _isAdding;
        [RelayCommand]
        public void AddProduct(Product productFromUi)
        {
            if (productFromUi is null || productFromUi.Id <= 0) return;
            
            var fresh = _productService.Get(productFromUi.Id);
            if (fresh is null || fresh.Stock <= 0) return;
            
            var vmRow = MyGroceryListItems.FirstOrDefault(i => i.ProductId == fresh.Id);

            if (vmRow is null)
            {
                var saved = _groceryListItemsService.Add(new GroceryListItem(0, GroceryList.Id, fresh.Id, 1)
                {
                    Product = fresh 
                });
                
                MyGroceryListItems.Add(saved);
            }
            else
            {
                vmRow.Amount += 1;
                _groceryListItemsService.Update(new GroceryListItem(vmRow.Id, GroceryList.Id, fresh.Id, vmRow.Amount));
            }
            
            fresh.Stock -= 1;
            _productService.Update(fresh); 
            
            if (fresh.Stock == 0)
            {
                var toRemove = AvailableProducts.FirstOrDefault(p => p.Id == fresh.Id);
                if (toRemove is not null) AvailableProducts.Remove(toRemove);
            }
            
            WeakReferenceMessenger.Default.Send(fresh);
        }
    }
}


        
    

        

