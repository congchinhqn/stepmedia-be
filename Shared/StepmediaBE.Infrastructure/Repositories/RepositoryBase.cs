using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace StepmediaBE.Infrastructure.Repositories;

public interface IRepository<out T> where T : class
{
    StepmediaContext Context { get; }
    IQueryable<T> GetNoQueryFiltersSet();
}

public abstract class RepositoryBase<T> : IRepository<T> where T : class
{
    #region Constructors

    protected RepositoryBase(StepmediaContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    #endregion

    #region Properties

    public StepmediaContext Context { get; }
    public IQueryable<T> GetNoQueryFiltersSet()
    {
        return DbSet.IgnoreQueryFilters();
    }

    protected DbSet<T> DbSet => Context.Set<T>();

    #endregion
}