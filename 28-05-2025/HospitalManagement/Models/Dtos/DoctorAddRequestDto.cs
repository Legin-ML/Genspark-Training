namespace HospitalManagement.Models.Dtos;

public class DoctorAddRequestDto
{
    public string Name { get; set; }
    public ICollection<SpecialityAddRequestDto>? Specialities { get; set; }
    public int YearsOfExperience { get; set; }
}