using ShopMigrationAPI.Models;

namespace ShopMigrationAPI.Interfaces.Services;

public interface ICategoryService
{
    IEnumerable<Category> GetAllCategories(int pageNumber, int pageSize);
    Category GetCategoryById(int id);
    void AddCategory(Category category);
    void UpdateCategory(Category category);
    void DeleteCategory(int id);
}