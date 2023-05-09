using System;
using System.Threading;
using System.Threading.Tasks;
using FAI.Domain.Models;
using FluentValidation;
using MediatR;
using Metatrade.Core.ErrorDefinitions;
using Metatrade.Core.Exceptions;
using Metatrade.Core.Extensions;
using Metatrade.OrderService.Dtos;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using StepmediaBE.Infrastructure.Repositories;

namespace StepmediaBE.OrderService.CommandHandlers.Customers;

public class UpdateCustomerCommand : IRequest<Unit>
{
    public long CustomerId { get; private set; }
    public string FullName { get; set; }
    public DateTime DOB { get; set; }
    public string Email { get; set; }

    public void SetCustomerId(long id)
    {
        CustomerId = id;
    }
    public void PrepareData()
    {
        FullName = string.IsNullOrEmpty(FullName) ? string.Empty : FullName.Trim();
        Email = string.IsNullOrEmpty(Email) ? string.Empty : Email.Trim();
    }
}

public class UpdateCustomerCommandValidator : BaseValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().WithError(new Error("missing_fullname", "Missing fullname"));
        RuleFor(x => x.DOB).NotEmpty().WithError(new Error("missing_dob", "Missing date of birth"));
        RuleFor(x => x.Email).NotEmpty().WithError(new Error("missing_email", "Missing email"));
    }
}

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Unit>
{
    #region Fields

    private readonly ILogger<UpdateCustomerCommandHandler> _logger;
    private readonly ICustomerRepository _customerRepository;

    #endregion

    public UpdateCustomerCommandHandler(ILogger<UpdateCustomerCommandHandler> logger,
        ICustomerRepository customerRepository)
    {
        _logger = logger;
        _customerRepository = customerRepository;
    }

    public async Task<Unit> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        command.PrepareData();
        var customer = await _customerRepository.GetAsync(command.CustomerId);
        if (customer == null)
        {
            throw new AppException(new Error("customer_not_exist", "Customer is not exist"));
        }

        if (command.FullName != customer.FullName)
            customer.SetFullName(command.FullName);

        if (command.DOB != customer.DOB)
            customer.SetDOB(command.DOB);

        if (command.Email != customer.Email)
            customer.SetEmail(command.Email);

        await _customerRepository.Context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Update customer successful");
        
        return Unit.Value;
    }
}