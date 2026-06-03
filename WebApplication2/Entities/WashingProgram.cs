namespace WebApplication2.Entities;

public class WashingProgram
{
    public int ProgramId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public int TemperatureCelsius { get; set; }

    public virtual ICollection<AvailableProgram> AvailablePrograms { get; set; } = new List<AvailableProgram>();
}
