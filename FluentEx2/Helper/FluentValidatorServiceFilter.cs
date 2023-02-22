using FluentEx2.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;

public class FluentValidationServiceFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public FluentValidationServiceFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.ActionArguments.FirstOrDefault(f => f.Value is IActionRequest);
        if (request.Value is not null)
        {
            var ct = context.ActionArguments.FirstOrDefault(f => f.Value is CancellationToken).Value;

            var cancellationToken = ct is not null ? (CancellationToken)ct : default;

            var validationResult = await ValidationRequest(request.Value, cancellationToken).ConfigureAwait(false);
            if (validationResult.IsValid is false)
            {
                context.Result = new ObjectResult(validationResult.Errors.Select(s => s.ErrorMessage).ToArray());
                return;
            }
        }
        await next();

    }

    private async Task<ValidationResult> ValidationRequest(object request, CancellationToken cancellationToken)
    {
        var typeofRequest = (request).GetType();
        var validatorType = typeof(IValidator<>).MakeGenericType(request.GetType());

        //var validatorService = _serviceProvider.GetService(typeof(IValidator<ProductSaveModel>));
        var validatorService = _serviceProvider.GetService(validatorType);
        if (validatorService is null) return new ValidationResult();

        var validatorMethod = validatorType.GetMethod(nameof(IValidator.ValidateAsync));
        if (validatorMethod is null) return new ValidationResult();

        var task = (Task)validatorMethod.Invoke(validatorService, new object[] { request, cancellationToken });
        await task.ConfigureAwait(false);

        var resultProperty = task.GetType().GetProperty("Result");
        return resultProperty?.GetValue(task) as ValidationResult ?? new ValidationResult();

    }
}