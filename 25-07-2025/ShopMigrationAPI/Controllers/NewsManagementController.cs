using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopMigrationAPI.Interfaces.Services;
using ShopMigrationAPI.Models;
using ShopMigrationAPI.Models.DTOs;
using ShopMigrationAPI.Services;

namespace ShopMigrationAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NewsManagementController : ControllerBase
    {
        private readonly NewsManagementService _newsService;
        private readonly IMapper _mapper;

        public NewsManagementController( NewsManagementService newsService, IMapper mapper)
        {
            _newsService = newsService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var newsList = _newsService.GetNews(page, pageSize);
            return Ok(_mapper.Map<IEnumerable<NewsDTO>>(newsList));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var news = _newsService.GetNewsById(id);
            if (news == null) return NotFound();
            return Ok(_mapper.Map<NewsDTO>(news));
        }

        [HttpPost]
        public IActionResult Create([FromBody] NewsDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var news = _mapper.Map<News>(dto);
            _newsService.CreateNews(news);

            return CreatedAtAction(nameof(GetById), new { id = news.Newsid }, dto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] NewsDTO dto)
        {
            if (dto == null || dto.NewsId != id) return BadRequest("ID mismatch");

            var existing = _newsService.GetNewsById(id);
            if (existing == null) return NotFound();

            var updated = _mapper.Map<News>(dto);
            _newsService.UpdateNews(updated);

            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _newsService.GetNewsById(id);
            if (existing == null) return NotFound();

            _newsService.DeleteNews(id);
            return NoContent();
        }

        [HttpGet("export/csv")]
        public IActionResult ExportCsv()
        {
            var newsList = _newsService.GetAllNewsWithUsers();
            var csv = string.Join("\n", newsList.Select(n =>
                $"\"{n.Newsid}\",\"{n.Title}\",\"{n.Shortdescription}\",\"{n.Createddate}\",\"{n.Status}\""));

            var bytes = System.Text.Encoding.UTF8.GetBytes($"\"NewsId\",\"Title\",\"ShortDescription\",\"CreatedDate\",\"Status\"\n{csv}");
            return File(bytes, "text/csv", $"NewsListing_{DateTime.Now:yyyyMMddHHmmss}.csv");
        }

        [HttpGet("export/excel")]
        public IActionResult ExportExcel()
        {
            // TODO: FIX WITH PERMANENT SOLUTION. THIS IS A PLACEHOLDER
            var newsList = _newsService.GetAllNewsWithUsers();
            var content = string.Join("\n", newsList.Select(n =>
                $"{n.Newsid}\t{n.Title}\t{n.Shortdescription}\t{n.Createddate}\t{n.Status}"));
            var bytes = System.Text.Encoding.UTF8.GetBytes($"NewsId\tTitle\tShortDescription\tCreatedDate\tStatus\n{content}");
            return File(bytes, "application/vnd.ms-excel", $"NewsListing_{DateTime.Now:yyyyMMddHHmmss}.xls");
        }
    }
}
