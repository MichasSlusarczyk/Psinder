using Microsoft.AspNetCore.Mvc.Filters;
using Psinder.DB.Common.Repositories.UnitOfWorks;

namespace Psinder.API.Common.Filters;

public class RunAsTransactionFilter : IAsyncActionFilter
{
    private readonly ILogger<RunAsTransactionFilter> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public RunAsTransactionFilter(ILogger<RunAsTransactionFilter> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        try
        {
            _logger.LogInformation($"Starting trnsaction");
            await _unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted, CancellationToken.None);

            var executedAction = await next();

            if (executedAction.Exception != null)
            {
                _logger.LogWarning($"An exception occured. Rolling back transaction.");
                await _unitOfWork.RollbackTransaction(CancellationToken.None);
            }
            else if (executedAction.Canceled)
            {
                _logger.LogWarning($"Acton was canceled. Rolling back transaction.");
                await _unitOfWork.RollbackTransaction(CancellationToken.None);
            }
            else
            {
                await _unitOfWork.CommitTransaction(CancellationToken.None);
            }
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransaction(CancellationToken.None);
            _logger.LogInformation(ex, $"An error occured. Rolling back transaction.");
            throw;
        }
    }
}
