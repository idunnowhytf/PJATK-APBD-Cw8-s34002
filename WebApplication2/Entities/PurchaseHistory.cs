namespace WebApplication2.Entities;

public class PurchaseHistory
{
    public int AvailableProgramId { get; set; }
    public int CustomerId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int? Rating { get; set; }

    public virtual AvailableProgram AvailableProgram { get; set; } = null!;
    public virtual Customer Customer { get; set; } = null!;
}
