using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.DTOs;
using WebApplication2.Entities;
using WebApplication2.Exceptions;

namespace WebApplication2.Services;

public class WashingMachineService(DatabaseContext context) : IWashingMachineService
{
    public async Task<CustomerResponse> GetCustomerPurchasesAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
            throw new BadRequestException("Invalid customer ID.");

        var customer = await context.Customers
            .Include(c => c.PurchaseHistories)
                .ThenInclude(ph => ph.AvailableProgram)
                    .ThenInclude(ap => ap.WashingMachine)
            .Include(c => c.PurchaseHistories)
                .ThenInclude(ph => ph.AvailableProgram)
                    .ThenInclude(ap => ap.Program)
            .FirstOrDefaultAsync(c => c.CustomerId == id, cancellationToken);

        if (customer == null)
            throw new NotFoundException($"Customer with ID {id} not found.");

        return new CustomerResponse
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            PhoneNumber = customer.PhoneNumber,
            Purchases = customer.PurchaseHistories.Select(ph => new PurchaseResponse
            {
                Date = ph.PurchaseDate,
                Rating = ph.Rating,
                Price = ph.AvailableProgram.Price,
                WashingMachine = new WashingMachineResponse
                {
                    Serial = ph.AvailableProgram.WashingMachine.SerialNumber,
                    MaxWeight = ph.AvailableProgram.WashingMachine.MaxWeight
                },
                Program = new ProgramResponse
                {
                    Name = ph.AvailableProgram.Program.Name,
                    Duration = ph.AvailableProgram.Program.DurationMinutes
                }
            }).ToList()
        };
    }

    public async Task AddWashingMachineAsync(CreateWashingMachineRequest request, CancellationToken cancellationToken)
    {
        if (request == null || request.WashingMachine == null || request.AvailablePrograms == null)
            throw new BadRequestException("Invalid request data.");

        if (request.WashingMachine.MaxWeight < 8)
            throw new BadRequestException("Max weight cannot be less than 8.");

        if (request.AvailablePrograms.Any(p => p.Price > 25))
            throw new BadRequestException("Price cannot exceed 25.");

        var exists = await context.WashingMachines
            .AnyAsync(w => w.SerialNumber == request.WashingMachine.SerialNumber, cancellationToken);
        
        if (exists)
            throw new ConflictException($"Washing machine with serial number {request.WashingMachine.SerialNumber} already exists.");

        var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var washingMachine = new WashingMachine
            {
                MaxWeight = request.WashingMachine.MaxWeight,
                SerialNumber = request.WashingMachine.SerialNumber
            };

            await context.WashingMachines.AddAsync(washingMachine, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            foreach (var programReq in request.AvailablePrograms)
            {
                var program = await context.Programs
                    .FirstOrDefaultAsync(p => p.Name == programReq.ProgramName, cancellationToken);

                if (program == null)
                    throw new NotFoundException($"Program with name {programReq.ProgramName} not found.");

                var availableProgram = new AvailableProgram
                {
                    WashingMachineId = washingMachine.WashingMachineId,
                    ProgramId = program.ProgramId,
                    Price = programReq.Price
                };

                await context.AvailablePrograms.AddAsync(availableProgram, cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
