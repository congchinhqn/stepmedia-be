using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FAI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace StepmediaBE.Infrastructure.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    #region Methods

    void Add(Customer customer);
    void Remove(Customer customer);
    Task<Customer> GetAsync(long id); 
    Task<List<Customer>> GetAsync(IEnumerable<long> ids);

    Task<bool> GetExistAsync(string email);

    #endregion
}

public sealed class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
{
    public CustomerRepository(StepmediaContext context) : base(context)
    {
    }

    public void Add(Customer customer)
    {
        DbSet.Add(customer);
    }
    
    public void Remove(Customer customer)
    {
        DbSet.Remove(customer);
    }

    public Task<Customer> GetAsync(long id)
    {
        return DbSet.FirstOrDefaultAsync(e => e.CustomerId == id);
    }

    public Task<List<Customer>> GetAsync(IEnumerable<long> ids)
    {
        var idsList = ids.ToList();
        return DbSet.Where(x => idsList.Contains(x.CustomerId)).ToListAsync();
    }

    public Task<bool> GetExistAsync(string email)
    {
        return DbSet.AnyAsync(e => e.Email == email);
    }
}