using System.ComponentModel.DataAnnotations;

namespace WebApplication2.DTOs;

public class CreateWashingMachineRequest
{
    [Required]
    public WashingMachineRequest WashingMachine { get; set; } = null!;

    [Required]
    public List<AvailableProgramRequest> AvailablePrograms { get; set; } = new();
}

public class WashingMachineRequest
{
    [Required]
    [Range(8.0, double.MaxValue, ErrorMessage = "MaxWeight must be at least 8")]
    public decimal MaxWeight { get; set; }

    [Required]
    [MaxLength(100)]
    public string SerialNumber { get; set; } = string.Empty;
}

public class AvailableProgramRequest
{
    [Required]
    [MaxLength(50)]
    public string ProgramName { get; set; } = string.Empty;

    [Required]
    [Range(0.0, 25.0, ErrorMessage = "Price cannot exceed 25")]
    public decimal Price { get; set; }
}
