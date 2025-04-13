using ElectronicVoting.Admin.Infrastructure.EntityFramework;
using MediatR;

namespace ElectronicVoting.Admin.Infrastructure.MediatR;

public class TransactionBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        TResponse response;
        if (request is not ITransaction) return await next();

        try
        {
            await unitOfWork.BeginTransaction(cancellationToken);
            response = await next();
            await unitOfWork.Commit(cancellationToken);
        }
        catch (Exception e)
        {
            await unitOfWork.Rollback(cancellationToken);
            throw;
        }

        return response;
    }
}