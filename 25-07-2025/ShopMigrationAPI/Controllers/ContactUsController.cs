using Microsoft.AspNetCore.Mvc;
using ShopMigrationAPI.Models;
using ShopMigrationAPI.Services;

namespace ShopMigrationAPI.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class ContactUsController : ControllerBase
    {
        private readonly ContactUsService _contactUsService;

        public ContactUsController(ContactUsService contactUsService)
        {
            _contactUsService = contactUsService;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<Contactu>> GetAllContacts()
        {
            var contacts = _contactUsService.GetAllContacts();
            return Ok(contacts);
        }
        
        [HttpGet("{id}")]
        public ActionResult<Contactu> GetContactById(int id)
        {
            var contact = _contactUsService.GetContactById(id);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }
        
        [HttpPost]
        public ActionResult CreateContact([FromBody] Contactu contact)
        {
            if (contact == null)
            {
                return BadRequest("Invalid contact data.");
            }

            _contactUsService.CreateContact(contact);
            
            return CreatedAtAction(nameof(GetContactById), new { id = contact.Id }, contact);
        }
        
        [HttpPut("{id}")]
        public ActionResult UpdateContact(int id, [FromBody] Contactu contact)
        {
            if (contact == null || contact.Id != id)
            {
                return BadRequest("Invalid contact data.");
            }

            var existingContact = _contactUsService.GetContactById(id);
            if (existingContact == null)
            {
                return NotFound();
            }

            _contactUsService.UpdateContact(contact);

            return NoContent();  
        }
        
        [HttpDelete("{id}")]
        public ActionResult DeleteContact(int id)
        {
            var contact = _contactUsService.GetContactById(id);
            if (contact == null)
            {
                return NotFound();
            }

            _contactUsService.DeleteContact(id);

            return NoContent(); 
        }
        
        /*[HttpPost("ValidateCaptcha")]
        public ActionResult ValidateCaptcha([FromBody] CaptchaRequest captchaRequest)
        {
            const string secret = "your-captcha-secret";
            var captchaResponse = CaptchaValidator.ValidateCaptcha(captchaRequest.Response, secret);

            if (!captchaResponse.Success)
            {
                return BadRequest(captchaResponse.ErrorMessage);
            }

            return Ok("Captcha validated successfully.");
        }*/
    }
}
