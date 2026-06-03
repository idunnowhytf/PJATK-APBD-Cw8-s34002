namespace WebApplication2.Entities;

public class AvailableProgram
{
    public int AvailableProgramId { get; set; }
    public int WashingMachineId { get; set; }
    public int ProgramId { get; set; }
    public decimal Price { get; set; }

    public virtual WashingMachine WashingMachine { get; set; } = null!;
    public virtual WashingProgram Program { get; set; } = null!;
    public virtual ICollection<PurchaseHistory> PurchaseHistories { get; set; } = new List<PurchaseHistory>();
}
