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

public class DeleteCustomerCommand : IRequest<Unit>
{
    public long CustomerId { get; set; }
}

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Unit>
{
    #region Fields

    private readonly ILogger<DeleteCustomerCommandHandler> _logger;
    private readonly ICustomerRepository _customerRepository;

    #endregion

    public DeleteCustomerCommandHandler(ILogger<DeleteCustomerCommandHandler> logger,
        ICustomerRepository customerRepository)
    {
        _logger = logger;
        _customerRepository = customerRepository;
    }

    public async Task<Unit> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetAsync(command.CustomerId);
        if (customer == null)
        {
            throw new AppException(new Error("customer_not_exist", "Customer is not exist"));
        }

        _customerRepository.Remove(customer);
        await _customerRepository.Context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Delete customer successful");

        return Unit.Value;
    }
}