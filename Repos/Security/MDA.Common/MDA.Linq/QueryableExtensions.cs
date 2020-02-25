//-----------------------------------------------------------------------
// <copyright file="QueryableExtensions.cs" company="MDA Corporation">
//     Copyright (c) MDA Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MDA.Linq
{
    using Microsoft.Linq.Translations;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic;

    /// <summary>
    /// Queryable Extensions
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Applies data processing (paging, sorting and filtering) over IQueryable using Dynamic Linq.
        /// </summary>
        /// <typeparam name="T">The type of the IQueryable</typeparam>
        /// <param name="queryable">The IQueryable which should be processed.</param>
        /// <param name="take">Specifies how many items to take. Configurable via the pageSize setting of the Kendo DataSource.</param>
        /// <param name="skip">Specifies how many items to skip.</param>
        /// <param name="sort">Specifies the current sort order.</param>
        /// <param name="filter">Specifies the current filter.</param>
        /// <returns>A DataSourceResult object populated from the processed IQueryable.</returns>
        public static DataSourceResult<T> ToDataSourceResult<T>(this IQueryable<T> queryable, int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int intTotalIn = -1)
        {
            // Filter the data first
            queryable = Filter(queryable, ParseFilter(typeof(T), filter));

            // Calculate the total number of records (needed for paging)
            var total = intTotalIn >= 0 ? intTotalIn : queryable.WithTranslations().Count();

            // Sort the data
            queryable = Sort(queryable, sort);

            // Finally page the data
            queryable = Page(queryable, take, skip);

            // Return the Result
            return new DataSourceResult<T>
            {
                Data = queryable.WithTranslations().ToList(),
                Total = total
            };
        }

        /// <summary>
        /// Applies data processing (sorting and filtering) over IQueryable using Dynamic Linq.
        /// </summary>
        /// <typeparam name="T">The type of the IQueryable</typeparam>
        /// <param name="queryable">The IQueryable which should be processed.</param>
        /// <param name="sort">Specifies the current sort order.</param>
        /// <param name="filter">Specifies the current filter.</param>
        /// <returns>A IEnumerable object populated from the processed IQueryable.</returns>
        public static IEnumerable<T> ToListResult<T>(this IQueryable<T> queryable, IEnumerable<Sort> sort, LinqFilter filter)
        {
            // Filter the data first
            queryable = Filter(queryable, ParseFilter(typeof(T), filter));

            // Sort the data
            queryable = Sort(queryable, sort);

            // Return the List
            return queryable.WithTranslations().ToList();
        }

        /// <summary>
        /// Applies data processing (paging and filtering) over IQueryable using Dynamic Linq.
        /// </summary>
        /// <typeparam name="T">The type of the IQueryable</typeparam>
        /// <param name="queryable">The IQueryable which should be processed.</param>
        /// <param name="take">Specifies how many items to take. Configurable via the pageSize setting of the Kendo DataSource.</param>
        /// <param name="filter">Specifies the current filter.</param>
        /// <param name="selectedField">Selected Field</param>
        /// <returns>A IEnumerable object populated from the processed IQueryable.</returns>
        public static IEnumerable<string> ToListResult<T>(this IQueryable<T> queryable, int take, LinqFilter filter, string selectedField)
        {
            // Set the Sort on the Selected Field
            var sort = new[] { new Sort { Field = selectedField, Dir = "asc" } };

            // Filter the data first
            queryable = Filter(queryable, ParseFilter(typeof(T), filter));

            // Sort the data
            queryable = Sort(queryable, sort);

            // Finally page the data
            queryable = Page(queryable, take, 0);

            // Return the List
            return queryable.Select(selectedField).Cast<string>().WithTranslations().ToList();
        }

        /// <summary>
        /// Applies data processing (filtering) over IQueryable using Dynamic Linq.
        /// </summary>
        /// <typeparam name="T">The type of the IQueryable</typeparam>
        /// <param name="queryable">The IQueryable which should be processed.</param>
        /// <param name="filter">Specifies the current filter.</param>
        /// <returns>A IQueryable object.</returns>
        private static IQueryable<T> Filter<T>(IQueryable<T> queryable, LinqFilter filter)
        {
            // Set the Filter for use
            if (filter != null)
            {
                filter = (filter.Filters == null) ? null : filter;
            }

            if (filter != null)
            {
                // Collect a flat list of all filters
                var filters = filter.All();

                // Get all filter values as array (needed by the Where method of Dynamic Linq)
                var values = filters.Select(f => f.Value).ToArray();

                // Create a predicate expression e.g. Field1 = @0 And Field2 > @1
                var predicate = filter.ToExpression(filters);

                // Use the Where method of Dynamic Linq to filter the data
                queryable = queryable.Where(predicate, values);
            }

            return queryable;
        }

        /// <summary>
        /// Get Property Type
        /// </summary>
        /// <param name="type">Data Type</param>
        /// <param name="propertyName">Property Name</param>
        /// <returns>Property Type</returns>
        private static Type GetPropertyType(Type type, string propertyName)
        {
            foreach (var property in propertyName.Split('.').Select(x => type.GetProperty(x)))
            {
                type = GetUnderlyingType(property.PropertyType);
            }

            return type;
        }

        /// <summary>
        /// Return underlying type if type is nullable otherwise return the type
        /// </summary>
        /// <param name="type">Data Type</param>
        /// <returns>Type of T</returns>
        private static Type GetUnderlyingType(Type type)
        {
            if (type != null && IsNullable(type))
            {
                if (!type.IsValueType)
                {
                    return type;
                }

                return Nullable.GetUnderlyingType(type);
            }

            return type;
        }

        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        /// <param name="type">Data Type</param>
        /// <returns>True or False</returns>
        private static bool IsNullable(Type type)
        {
            return !type.IsValueType || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Applies data processing (paging) over IQueryable using Dynamic Linq.
        /// </summary>
        /// <typeparam name="T">The type of the IQueryable</typeparam>
        /// <param name="queryable">The IQueryable which should be processed.</param>
        /// <param name="take">
        /// Specifies how many items to take. Configurable via the pageSize setting of the Kendo DataSource.
        /// </param>
        /// <param name="skip">Specifies how many items to skip.</param>
        /// <returns>A IQueryable object.</returns>
        private static IQueryable<T> Page<T>(IQueryable<T> queryable, int take, int skip)
        {
            return queryable.Skip(skip).Take(take);
        }

        /// <summary>
        /// Parse Date
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>Filter List</returns>
        private static List<LinqFilter> ParseDate(LinqFilter filter)
        {
            var startDate = DateTime.Parse(filter.Value.ToString());
            var endDate = startDate.AddDays(1).AddTicks(-1);

            var filterList = new List<LinqFilter>();
            if (filter.Operator == "eq" || filter.Operator == "neq" || filter.Operator == "contains")
            {
                filterList.Add(new LinqFilter { Field = filter.Field, Operator = filter.Operator == "neq" ? "lt" : "gte", Value = startDate });
                filterList.Add(new LinqFilter { Field = filter.Field, Operator = filter.Operator == "neq" ? "gt" : "lte", Value = endDate });
            }
            else
            {
                filterList.Add(new LinqFilter
                {
                    Field = filter.Field,
                    Operator = filter.Operator,
                    Value = filter.Operator == "gte" || filter.Operator == "lt" ? startDate : endDate
                });
            }

            return filterList;
        }

        /// <summary>
        /// Parse Filter
        /// </summary>
        /// <param name="T">Object Type</param>
        /// <param name="linqFilter">Filter to Parse</param>
        private static LinqFilter ParseFilter(Type T, LinqFilter linqFilter)
        {
            if (linqFilter != null)
            {
                if (linqFilter.Filters != null)
                {
                    foreach (var filter in linqFilter.Filters)
                    {
                        ParseFilter(T, filter);
                    }

                    var filters = linqFilter.Filters.Where(x => x.Field != null || x.Filters != null).Select(x => x).ToList();
                    linqFilter.Filters = (filters.Any()) ? filters : null;
                }

                if (linqFilter.Field != null)
                {
                    var fieldType = GetPropertyType(T, linqFilter.Field);
                    var stringComparisonForEmpty = new[] { "isnull", "isempty" };
                    var stringComparisonForNotEmpty = new[] { "isnotnull", "isnotempty" };

                    if (fieldType != typeof(string))
                    {
                        var stringComparison = new[] { "startswith", "endswith", "contains", "doesnotcontain" };
                        linqFilter.Operator = stringComparison.Contains(linqFilter.Operator) ? "eq" : linqFilter.Operator;

                        try
                        {
                            if (fieldType == typeof(Int32)) { linqFilter.Value = Int32.Parse(linqFilter.Value.ToString()); }
                            else if (fieldType == typeof(Int64)) { linqFilter.Value = Int64.Parse(linqFilter.Value.ToString()); }
                            else if (fieldType == typeof(Int16)) { linqFilter.Value = Int16.Parse(linqFilter.Value.ToString()); }
                            else if (fieldType == typeof(Boolean)) { linqFilter.Value = Boolean.Parse(linqFilter.Value.ToString()); }
                            else if (fieldType == typeof(Byte)) { linqFilter.Value = Byte.Parse(linqFilter.Value.ToString()); }
                            else if (fieldType == typeof(DateTime))
                            {
                                linqFilter.Logic = linqFilter.Operator == "neq" ? "OR" : "AND";
                                linqFilter.Filters = ParseDate(linqFilter);
                            }
                            else if (fieldType == typeof(Decimal)) { linqFilter.Value = Decimal.Parse(linqFilter.Value.ToString()); }
                            else if (fieldType == typeof(Double)) { linqFilter.Value = Double.Parse(linqFilter.Value.ToString()); }
                        }
                        catch
                        {
                            linqFilter.Field = null;
                        }
                    }
                    else if (linqFilter.Value != null && (stringComparisonForEmpty.Contains(linqFilter.Operator) || stringComparisonForNotEmpty.Contains(linqFilter.Operator)))
                    {
                        linqFilter.Operator = stringComparisonForEmpty.Contains(linqFilter.Operator) ? "doesnotcontain" : "contains";
                    }
                }
            }

            return linqFilter;
        }

        /// <summary>
        /// Applies data processing (sorting) over IQueryable using Dynamic Linq.
        /// </summary>
        /// <typeparam name="T">The type of the IQueryable</typeparam>
        /// <param name="queryable">The IQueryable which should be processed.</param>
        /// <param name="sort">Specifies the current sort order.</param>
        /// <returns>A IQueryable object.</returns>
        private static IQueryable<T> Sort<T>(IQueryable<T> queryable, IEnumerable<Sort> sort)
        {
            var sorts = sort as IList<Sort>;
            if (sorts != null && (sorts.Any()))
            {
                // Create ordering expression e.g. Field1 asc, Field2 desc
                var ordering = string.Join(",", sorts.Select(s => s.ToExpression()));

                // Use the OrderBy method of Dynamic Linq to sort the data
                return queryable.OrderBy(ordering);
            }

            return queryable;
        }
    }
}