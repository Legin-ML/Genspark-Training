public class Doctor
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Doctor(int id, string name)
    {
        Id = id;
        Name = name;
    }
}