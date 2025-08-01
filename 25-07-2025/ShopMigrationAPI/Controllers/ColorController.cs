using Microsoft.AspNetCore.Mvc;
using ShopMigrationAPI.Interfaces.Services;
using ShopMigrationAPI.Models;
using ShopMigrationAPI.Services;

namespace ShopMigrationAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ColorController : ControllerBase
    {
        private readonly IColorService _colorService;

        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<Color>> GetColors(int page = 1, int pageSize = 5)
        {
            var colors = _colorService.GetColors(page, pageSize);
            return Ok(colors);
        }


        [HttpGet("{id}")]
        public ActionResult<Color> GetColor(int id)
        {
            var color = _colorService.GetColor(id);
            if (color == null)
            {
                return NotFound();
            }
            return Ok(color);
        }

        [HttpPost]
        public IActionResult CreateColor([FromBody] Color color)
        {
            if (color == null)
            {
                return BadRequest("Invalid color data.");
            }

            _colorService.CreateColor(color);
            return CreatedAtAction(nameof(GetColor), new { id = color.Colorid }, color);
        }
        
        [HttpPut("{id}")]
        public IActionResult UpdateColor(int id, [FromBody] Color color)
        {
            if (color == null || color.Colorid != id)
            {
                return BadRequest("Color ID mismatch.");
            }

            var existingColor = _colorService.GetColor(id);
            if (existingColor == null)
            {
                return NotFound();
            }

            _colorService.UpdateColor(color);
            return Ok(color);
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeleteColor(int id)
        {
            var existingColor = _colorService.GetColor(id);
            if (existingColor == null)
            {
                return NotFound();
            }

            _colorService.DeleteColor(id);
            return NoContent();
        }
    }
}
