public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string? Reason { get; set; }

    public Patient(int id, string name, int age, string? reason)
    {
        Id = id;
        Name = name;
        Age = age;
        Reason = reason;
    }
}