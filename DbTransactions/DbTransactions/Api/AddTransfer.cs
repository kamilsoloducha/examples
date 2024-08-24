using System.Net;
using DbTransactions.Infrastructure;
using DbTransactions.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbTransactions.Api;

public static class Endpoints
{
    public static WebApplication AddTransfer(this WebApplication host)
    {
        host.MapPost("addtransfer",
            async ([FromBody] AddTransferRequest request, LocalDbContext dbContext,
                CancellationToken cancellationToken) =>
            {
                var newTransfer = new Transfer
                {
                    SourceAccount = request.SourceAccount,
                    DestinationAccount = request.DestinationAccount,
                    Shares = request.Shares
                };

                await dbContext.AddAsync(newTransfer, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);

                return Results.Ok(newTransfer.ContractNumber);
            });
        return host;
    }

    public static WebApplication UpdateStatus(this WebApplication host)
    {
        host.MapPut("processtransfer", async ([FromBody] ProcessTransferRequest request, LocalDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var transfer =
                await dbContext.Transfers.FirstOrDefaultAsync(x => x.ContractNumber == request.ContractNumber,
                    cancellationToken);

            if (transfer is null)
            {
                return Results.NotFound();
            }

            transfer.Status = TransferStatus.Failed;

            await dbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok();
        });
        return host;
    }

    public static WebApplication UpdateStatusTransaction(this WebApplication host)
    {
        host.MapPut("processtransfer/transaction", async ([FromBody] ProcessTransferRequest request,
            LocalDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var transfer =
                    await dbContext.Transfers.FirstOrDefaultAsync(x => x.ContractNumber == request.ContractNumber,
                        cancellationToken);

                if (transfer is null)
                {
                    return Results.NotFound();
                }

                transfer.Status = TransferStatus.Created;

                await dbContext.SaveChangesAsync(cancellationToken);

                throw new Exception();
                
                await transaction.CommitAsync(cancellationToken);
                return Results.Ok();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        });
        return host;
    }
    
    public static WebApplication GetTransfer(this WebApplication host)
    {
        host.MapGet("transfer/{contractNumber}", async ([FromRoute] long contractNumber, LocalDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var transfer =
                await dbContext.Transfers.FirstOrDefaultAsync(x => x.ContractNumber == contractNumber,
                    cancellationToken);
            return transfer is null ? Results.NotFound() : Results.Ok(transfer);  
        });
        return host;
    }
}

public record AddTransferRequest(long SourceAccount, long DestinationAccount, decimal Shares);

public record ProcessTransferRequest(long ContractNumber);