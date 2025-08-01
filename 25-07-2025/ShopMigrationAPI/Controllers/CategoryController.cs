using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopMigrationAPI.Interfaces.Services;
using ShopMigrationAPI.Models;
using ShopMigrationAPI.Models.DTOs;

namespace ShopMigrationAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCategories(int page = 1, int pageSize = 5)
        {
            var categories = _categoryService.GetAllCategories(page, pageSize);
            var dtoList = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
            return Ok(dtoList);
        }

        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null) return NotFound();
            
            var dto = _mapper.Map<CategoryDTO>(category);
            return Ok(dto);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryDTO dto)
        {
            if (dto == null) return BadRequest("Invalid category data.");

            var categoryEntity = _mapper.Map<Category>(dto);
            _categoryService.AddCategory(categoryEntity);
            
            var createdDto = _mapper.Map<CategoryDTO>(categoryEntity);
            return CreatedAtAction(nameof(GetCategory), new { id = categoryEntity.Categoryid }, createdDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] CategoryDTO dto)
        {
            if (dto == null || dto.Categoryid != id)
                return BadRequest("Category ID mismatch.");

            var existing = _categoryService.GetCategoryById(id);
            if (existing == null) return NotFound();

            var entityToUpdate = _mapper.Map<Category>(dto);
            _categoryService.UpdateCategory(entityToUpdate);

            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var existing = _categoryService.GetCategoryById(id);
            if (existing == null) return NotFound();

            _categoryService.DeleteCategory(id);
            return NoContent();
        }
    }
}
