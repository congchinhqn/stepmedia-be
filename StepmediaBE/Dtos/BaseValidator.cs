using System.Linq;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Metatrade.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Metatrade.OrderService.Dtos;

public class BaseValidator<T> : AbstractValidator<T>, IValidatorInterceptor
{
    public IValidationContext BeforeMvcValidation(ControllerContext controllerContext,
        IValidationContext commonContext)
    {
        return commonContext;
    }

    public ValidationResult AfterMvcValidation(ControllerContext controllerContext,
        IValidationContext commonContext,
        ValidationResult result)
    {
        // TODO: Should `result.Errors` be always available in case `result` is invalid? Need to check further
        if (!result.IsValid)
        {
            var errorResult = result.Errors.First();
            throw new DomainException(errorResult.ErrorMessage, errorResult.ErrorCode);
        }

        return result;
    }
}