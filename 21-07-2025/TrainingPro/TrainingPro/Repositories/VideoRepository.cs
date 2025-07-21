using Microsoft.EntityFrameworkCore;
using TrainingPro.Contexts;
using TrainingPro.Models;

namespace TrainingPro.Repositories;

public class VideoRepository
{

    private readonly TrainingDBContext _context;
    private readonly ILogger<VideoRepository> _logger;
    private readonly DbSet<TrainingVideo> _set;
    
    public VideoRepository(TrainingDBContext context, ILogger<VideoRepository> logger)
    {
        _context = context;
        _logger = logger;
        _set = context.Set<TrainingVideo>();
    }
    public async Task<TrainingVideo> GetAsync(int id)
    {
        var value = await _set.FindAsync(id);
        if (value == null)
        {
            _logger.LogError($"Video with id {id} was not found");
            throw new KeyNotFoundException($"Video with id {id} was not found");
        }
        
        _logger.LogInformation($"Video with id {id} was found");
        return value;
    }

    public async Task<List<TrainingVideo>> GetAllAsync()
    {
        _logger.LogInformation($"All videos were found");
        return  await _set.ToListAsync();;
    }

    public async Task<TrainingVideo> AddAsync(TrainingVideo value)
    {
        await _set.AddAsync(value);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Video with id {value.Id} was added");
        return value;
    }
}