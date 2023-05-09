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

public class CreateCustomerCommand : IRequest<Unit>
{
    public string FullName { get; set; }
    public DateTime DOB { get; set; }
    public string Email { get; set; }

    public void PrepareData()
    {
        FullName = string.IsNullOrEmpty(FullName) ? string.Empty : FullName.Trim();
        Email = string.IsNullOrEmpty(Email) ? string.Empty : Email.Trim();
    }
}

public class CreateCustomerCommandValidator : BaseValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().WithError(new Error("missing_fullname", "Missing fullname"));
        RuleFor(x => x.DOB).NotEmpty().WithError(new Error("missing_dob", "Missing date of birth"));
        RuleFor(x => x.Email).NotEmpty().WithError(new Error("missing_email", "Missing email"));
    }
}

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Unit>
{
    #region Fields

    private readonly ILogger<CreateCustomerCommandHandler> _logger;
    private readonly ICustomerRepository _customerRepository;

    #endregion

    public CreateCustomerCommandHandler(ILogger<CreateCustomerCommandHandler> logger,
        ICustomerRepository customerRepository)
    {
        _logger = logger;
        _customerRepository = customerRepository;
    }

    public async Task<Unit> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        command.PrepareData();
        if (await _customerRepository.GetExistAsync(command.Email))
        {
            throw new AppException(new Error("customer_exist", "Customer is exist"));
        }

        var customer = new Customer();
        customer.SetFullName(command.FullName);
        customer.SetDOB(command.DOB);
        customer.SetEmail(command.Email);
        _customerRepository.Add(customer);

        await _customerRepository.Context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Create customer successful");

        return Unit.Value;
    }
}