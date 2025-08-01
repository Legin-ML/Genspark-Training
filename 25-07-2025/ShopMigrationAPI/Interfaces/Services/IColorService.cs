using ShopMigrationAPI.Models;

namespace ShopMigrationAPI.Interfaces.Services;

public interface IColorService
{
    IEnumerable<Color> GetColors(int page = 1, int pageSize = 5);
    Color GetColor(int id);
    void CreateColor(Color color);
    void UpdateColor(Color color);
    void DeleteColor(int id);
}