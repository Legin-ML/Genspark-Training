using System.Data;
using ShopMigrationAPI.Interfaces.Repositories;
using ShopMigrationAPI.Interfaces.Services;
using ShopMigrationAPI.Models;

namespace ShopMigrationAPI.Services;

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;

    public CategoryService(IRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public IEnumerable<Category> GetAllCategories(int pageNumber, int pageSize)
    {
        var categories = _categoryRepository.GetAll();
        return categories.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }

    public Category GetCategoryById(int id)
    {
        return _categoryRepository.GetById(id);
    }

    public void AddCategory(Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }
        
        
        var exists = _categoryRepository.GetById(category.Categoryid);
        if (exists != null)
        {
            throw new DuplicateNameException($"Category with Id {category.Categoryid} already exists.");       
        }

        _categoryRepository.Add(category);
        _categoryRepository.Save();
    }

    public void UpdateCategory(Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }
        
        var exists = _categoryRepository.GetById(category.Categoryid);
        if (exists == null)
        {
            throw new KeyNotFoundException("Category does not exist.");       
        }

        _categoryRepository.Update(category);
        _categoryRepository.Save();
    }

    public void DeleteCategory(int id)
    {
        
        var exists = _categoryRepository.GetById(id);
        if (exists == null)
        {
            throw new KeyNotFoundException("Category does not exist.");       
        }
        _categoryRepository.Delete(id);
        _categoryRepository.Save();
    }
}