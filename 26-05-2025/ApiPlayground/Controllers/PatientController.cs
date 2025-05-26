using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/[controller]")]
public class PatientController : ControllerBase
{
    static List<Patient> patients = new List<Patient>
    {
        new Patient(1001, "Jane",25, "Fever"),
        new Patient(1002, "John",19, "")
    };

    [HttpGet]
    public ActionResult<IEnumerable<Patient>> GetPatients()
    {
        return Ok(patients);
    }

    [HttpPost]
    public ActionResult<Patient> PostPatient([FromBody] Patient patient)
    {
        patients.Add(patient);
        return Created("", patient);
    }

    // Gets ID from parameter - Refer to DoctorController.cs for an alternate method
    [HttpPut("{id}")]
    public ActionResult<Patient> PutPatient(int id, [FromBody] Patient patient)
    {
        var current = patients.FirstOrDefault(p => p.Id == id);

        if (current == null)
        {
            return NotFound();
        }

        current.Name = patient.Name;
        current.Age = patient.Age;
        current.Reason = patient.Reason;

        return Ok(current);
    }

    [HttpDelete("{id}")]
    public ActionResult<string> DeletePatient(int id)
    {
        var current = patients.FirstOrDefault(p => p.Id == id);

        if (current == null)
        {
            return NotFound();
        }

        patients.Remove(current);

        return Ok($"Patient with id {id} removed");
    }
}
