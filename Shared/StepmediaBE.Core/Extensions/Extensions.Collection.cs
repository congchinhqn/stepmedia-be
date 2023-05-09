using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Metatrade.Core.Extensions
{
    public static partial class MiscExtensions
    {
        // public static bool IsAny<T>(this IEnumerable<T> items) => items != null && items.Any();

        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            DataTable dtReturn = new DataTable();

            if (source == null) return dtReturn;
            // column names
            PropertyInfo[] oProps = null;

            foreach (var rec in source)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow
                if (oProps == null)
                {
                    oProps = rec.GetType().GetProperties();
                    foreach (var pi in oProps)
                    {
                        var colType = pi.PropertyType;

                        if (colType.IsNullable())
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        if (colType == typeof(bool))
                        {
                            colType = typeof(int);
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                var dr = dtReturn.NewRow();

                foreach (var pi in oProps)
                {
                    var value = pi.GetValue(rec, null) ?? DBNull.Value;
                    if (value is bool)
                    {
                        dr[pi.Name] = (bool)value ? 1 : 0;
                    }
                    else
                    {
                        dr[pi.Name] = value;
                    }
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }


        public static IPagedListDto<T> ToPagedList<T>(this IEnumerable<T> source, int totalCount, int pageIndex, int pageSize)
        {
            return new PagedListDto<T>(source, pageIndex, pageSize, totalCount);
        }
        
        public static IPagedListDto<T> ToEmptyPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
            return new PagedListDto<T>(source, pageIndex, pageSize, 0);
        }

        public static IPagedListDto<T> ToPagedList<T>(this IEnumerable<T> source, int totalCount, IPagingDto paging)
        {
            return new PagedListDto<T>(source, paging, totalCount);
        }
        
        public static IPagedListDto<T> ToEmptyPagedList<T>(this IEnumerable<T> source, IPagingDto paging)
        {
            return new PagedListDto<T>(source, paging, 0);
        }
        
        
        public static IPagedListDto<T, TData> ToPagedList<T, TData>(this IEnumerable<T> source, int totalCount, int pageIndex, int pageSize, TData data)
        {
            return new PagedListDto<T, TData>(source, pageIndex, pageSize, totalCount, data);
        }
        
        public static IPagedListDto<T, TData> ToEmptyPagedList<T, TData>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
            return new PagedListDto<T, TData>(source, pageIndex, pageSize, 0, default);
        }

        public static IPagedListDto<T, TData> ToPagedList<T, TData>(this IEnumerable<T> source, int totalCount, IPagingDto paging, TData data)
        {
            return new PagedListDto<T, TData>(source, paging, totalCount, data);
        }
        
        public static IPagedListDto<T, TData> ToEmptyPagedList<T, TData>(this IEnumerable<T> source,  IPagingDto paging)
        {
            return new PagedListDto<T, TData>(source, paging, 0, default);
        }

        public static IPagedListDto<T> ToPagedList<T>(this IEnumerable<T> source, Func<T, int> totalCountSelector,  int pageIndex, int pageSize)
        {
            var totalCount = 0;
            var first = source.FirstOrDefault();
            if (first != null)
                totalCount = totalCountSelector(first);

            return new PagedListDto<T>(source, pageIndex, pageSize, totalCount);
        }
        
        public static IPagedListDto<T> ToPagedList<T>(this IEnumerable<T> source, Func<T, int> totalCountSelector,  IPagingDto paging)
        {
            var totalCount = 0;
            var first = source.FirstOrDefault();
            if (first != null)
                totalCount = totalCountSelector(first);

            return new PagedListDto<T>(source, paging, totalCount);
        }

        public static IPagedListDto<T> ToPagedList<T>(this IEnumerable<T> source, IPagingDto paging)
        {
            return new PagedListDto<T>(source, paging, 0);
        }

        public static IPagedListDto<T, TData> ToPagedList<T, TData>(this IEnumerable<T> source, Func<T, int> totalCountSelector,  int pageIndex, int pageSize, TData data)
        {
            var totalCount = 0;
            var first = source.FirstOrDefault();
            if (first != null)
                totalCount = totalCountSelector(first);

            return new PagedListDto<T, TData>(source, pageIndex, pageSize, totalCount, data);
        }
        
        public static IPagedListDto<T, TData> ToPagedList<T, TData>(this IEnumerable<T> source, Func<T, int> totalCountSelector, IPagingDto paging, TData data)
        {
            var totalCount = 0;
            var first = source.FirstOrDefault();
            if (first != null)
                totalCount = totalCountSelector(first);

            return new PagedListDto<T, TData>(source, paging, totalCount, data);
        }
    }

    public interface IPagedListDto<out T>
    {
        public int PageIndex { get;  }
        public int PageSize { get; }
        public int TotalCount { get; }
        public int TotalPages { get; }
        public IEnumerable<T> Items { get; }
    }

    public interface IPagedListDto<out T, out TData>: IPagedListDto<T>
    {
        [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TData Data { get; }
    }


    public interface IPagingDto
    {
        public int PageIndex { get; }

        public int PageSize { get; }
    }
    
    
    internal class PagedListDto<T>: IPagedListDto<T>
    {
        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public int TotalPages { get; }
        public IEnumerable<T> Items { get; }

        public PagedListDto(IEnumerable<T> source, IPagingDto paging, int totalCount): this(source, paging.PageIndex, paging.PageSize,totalCount)
        {
        }

        public PagedListDto(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            Items = source;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
        }
    }


    internal class PagedListDto<T, TData> : PagedListDto<T>, IPagedListDto<T, TData>
    {
        public PagedListDto(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount, TData data):base(source, pageIndex, pageSize, totalCount)
        {
            Data = data;
        }
        
        
        public PagedListDto(IEnumerable<T> source, IPagingDto paging, int totalCount, TData data): base(source, paging,totalCount)
        {
            Data = data;
        }

        public TData Data { get; }
    }
}
