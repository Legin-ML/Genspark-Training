using ShopMigrationAPI.Interfaces.Repositories;
using ShopMigrationAPI.Interfaces.Services;
using ShopMigrationAPI.Models;

namespace ShopMigrationAPI.Services;

public class ColorService : IColorService
{
    private readonly IRepository<Color> _colorRepository;

    public ColorService(IRepository<Color> colorRepository)
    {
        _colorRepository = colorRepository;
    }

    public IEnumerable<Color> GetColors(int page = 1, int pageSize = 5)
    {
        var colors = _colorRepository.GetAll();
        return colors.Skip((page - 1) * pageSize).Take(pageSize);
    }

    public Color GetColor(int id)
    {
        return _colorRepository.GetById(id);
    }

    public void CreateColor(Color color)
    {
        if (color == null)
            throw new ArgumentNullException(nameof(color));
            
        _colorRepository.Add(color);
        _colorRepository.Save();
    }

    public void UpdateColor(Color color)
    {
        if (color == null)
            throw new ArgumentNullException(nameof(color));
            
        _colorRepository.Update(color);
        _colorRepository.Save();
    }

    public void DeleteColor(int id)
    {
        _colorRepository.Delete(id);
        _colorRepository.Save();
    }
}
