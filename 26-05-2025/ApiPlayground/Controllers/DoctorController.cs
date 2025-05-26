using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/[controller]")]
public class DoctorController : ControllerBase
{
    static List<Doctor> doctors = new List<Doctor>
    {
        new Doctor(1001, "Jane"),
        new Doctor(1002, "John")
    };

    [HttpGet]
    public ActionResult<IEnumerable<Doctor>> GetDoctors()
    {
        return Ok(doctors);
    }

    [HttpPost]
    public ActionResult<Doctor> PostDoctor([FromBody] Doctor doctor)
    {
        doctors.Add(doctor);
        return Created("", doctor);
    }

    // Gets Id from the request body - Refer to PatientController.cs for an alternate method
    [HttpPut]
    public ActionResult<Doctor> PutDoctor([FromBody] Doctor doctor)
    {
        var current = doctors.FirstOrDefault(doc => doc.Id == doctor.Id);

        if (current == null)
        {
            return NotFound();
        }

        doctors.Remove(current);
        doctors.Add(doctor);

        return Ok( doctor);
    }

    [HttpDelete("{id}")]
    public ActionResult<string> DeleteDoctor(int id)
    {
        var current = doctors.FirstOrDefault(doc => doc.Id == id);

        if (current == null)
        {
            return NotFound();
        }

        doctors.Remove(current);

        return Ok($"Doctor with id {id} removed");
    }
}