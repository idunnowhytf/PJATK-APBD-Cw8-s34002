namespace WebApplication2.Entities;

public class WashingMachine
{
    public int WashingMachineId { get; set; }
    public decimal MaxWeight { get; set; }
    public string SerialNumber { get; set; } = string.Empty;

    public virtual ICollection<AvailableProgram> AvailablePrograms { get; set; } = new List<AvailableProgram>();
}
