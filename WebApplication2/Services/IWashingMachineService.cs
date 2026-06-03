using WebApplication2.DTOs;

namespace WebApplication2.Services;

public interface IWashingMachineService
{
    Task<CustomerResponse> GetCustomerPurchasesAsync(int id, CancellationToken cancellationToken);
    Task AddWashingMachineAsync(CreateWashingMachineRequest request, CancellationToken cancellationToken);
}
