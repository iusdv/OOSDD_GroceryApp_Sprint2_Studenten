using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Services
{
    public interface IGroceryListItemsService
    {
        List<GroceryListItem> GetAll();
        List<GroceryListItem> GetAllOnGroceryListId(int groceryListId);
        GroceryListItem Add(GroceryListItem item);
        GroceryListItem? Update(GroceryListItem item);
        GroceryListItem? Delete(GroceryListItem item);
        GroceryListItem? Get(int id);
    }
}
