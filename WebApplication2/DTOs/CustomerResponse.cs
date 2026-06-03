namespace WebApplication2.DTOs;

public class CustomerResponse
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public List<PurchaseResponse> Purchases { get; set; } = new();
}

public class PurchaseResponse
{
    public DateTime Date { get; set; }
    public int? Rating { get; set; }
    public decimal Price { get; set; }
    public WashingMachineResponse WashingMachine { get; set; } = null!;
    public ProgramResponse Program { get; set; } = null!;
}

public class WashingMachineResponse
{
    public string Serial { get; set; } = string.Empty;
    public decimal MaxWeight { get; set; }
}

public class ProgramResponse
{
    public string Name { get; set; } = string.Empty;
    public int Duration { get; set; }
}
