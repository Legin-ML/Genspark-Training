using ShopMigrationAPI.Interfaces.Repositories;
using ShopMigrationAPI.Interfaces.Services;
using ShopMigrationAPI.Models;
using ShopMigrationAPI.Repositories;

namespace ShopMigrationAPI.Services
{
    public class NewsManagementService 
    {
        private readonly NewsRepository _newsRepository;

        public NewsManagementService(NewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public IEnumerable<News> GetNews(int page = 1, int pageSize = 10)
        {
            return _newsRepository.GetAll()
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public News GetNewsById(int id)
        {
            return _newsRepository.GetById(id);
        }

        public void CreateNews(News news)
        {
            _newsRepository.Add(news);
            _newsRepository.Save();
        }

        public void UpdateNews(News news)
        {
            _newsRepository.Update(news);
            _newsRepository.Save();
        }

        public void DeleteNews(int id)
        {
            _newsRepository.Delete(id);
            _newsRepository.Save();
        }
        
        public IEnumerable<News> GetAllNewsWithUsers(int page = 1, int pageSize = 10)
        {
            var fullList = _newsRepository.GetAllWithUsers();
            return fullList.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}