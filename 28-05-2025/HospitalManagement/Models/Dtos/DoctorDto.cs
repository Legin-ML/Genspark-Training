namespace HospitalManagement.Models.Dtos;

public class DoctorDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float YearsOfExperience { get; set; }
    public List<string> Specialities { get; set; }
}
