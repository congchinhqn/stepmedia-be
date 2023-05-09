using Metatrade.Core.ErrorDefinitions;
using FluentValidation;

namespace Metatrade.Core.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule, Error error)
        {
            return rule.WithMessage(error.Message).WithErrorCode(error.Code);
        }
    }
}