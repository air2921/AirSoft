using AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.Query;
using AirSoft.EntityFrameworkCore.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirSoft.EntityFrameworkCore.Extensions
{
    /// <summary>
    /// Provides extension methods for applying query configurations from a <see cref="ISingleQueryBuilder{TEntity}"/>
    /// to an <see cref="IQueryable{TEntity}"/> for single entity queries.
    /// </summary>
    /// <remarks>
    /// This static class contains helper methods to build Entity Framework queries for retrieving single entities
    /// with various options like includes, filtering, sorting and tracking behavior.
    /// </remarks>
    internal static class SingleQueryBuilderExtension
    {
        /// <summary>
        /// Applies all configured query modifications from the <see cref="ISingleQueryBuilder{TEntity}"/>
        /// to the provided <see cref="IQueryable{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The entity type, must inherit from <see cref="EntityBase"/></typeparam>
        /// <param name="query">The source query to apply modifications to</param>
        /// <param name="builder">The query builder containing all configuration settings</param>
        /// <returns>The modified query with all builder configurations applied</returns>
        /// <remarks>
        /// <para>This method applies configurations in the following order:</para>
        /// <list type="number">
        /// <item><description>Include paths for eager loading</description></item>
        /// <item><description>Auto include options (IgnoreAutoIncludes)</description></item>
        /// <item><description>Query filter options (IgnoreQueryFilters)</description></item>
        /// <item><description>Tracking behavior (AsNoTracking/AsTracking)</description></item>
        /// <item><description>Query splitting behavior (AsSplitQuery/AsSingleQuery)</description></item>
        /// <item><description>Filter conditions (Where clause)</description></item>
        /// <item><description>Selector (Select clause)</description></item>
        /// <item><description>Ordering (OrderBy/ThenBy clauses)</description></item>
        /// </list>
        /// </remarks>
        internal static IQueryable<TEntity> ApplyBuilder<TEntity>(this IQueryable<TEntity> query, ISingleQueryBuilder<TEntity> builder) where TEntity : EntityBase
        {
            if (builder.Joiner is not null && builder.Joiner.Paths.Count > 0)
            {
                foreach (var path in builder.Joiner.Paths.Distinct())
                {
                    query = query.Include(path);
                }
            }

            if (builder.IgnoreAutoInclude)
                query = query.IgnoreAutoIncludes();

            if (builder.IgnoreDefaultQueryFilters)
                query = query.IgnoreQueryFilters();

            if (builder.AsNoTracking)
                query = query.AsNoTracking();
            else
                query = query.AsTracking();

            if (builder.AsSplitQuery)
                query = query.AsSplitQuery();
            else
                query = query.AsSingleQuery();

            foreach (var filter in builder.Filters)
                query = query.Where(filter);

            if (builder.Selector is not null)
                query = query.Select(builder.Selector);

            if (builder.SortOptions.Count > 0)
            {
                var orderedQuery = builder.SortOptions[0].Descending
                    ? query.OrderByDescending(builder.SortOptions[0].Expression)
                    : query.OrderBy(builder.SortOptions[0].Expression);

                for (int i = 1; i < builder.SortOptions.Count; i++)
                {
                    orderedQuery = builder.SortOptions[i].Descending
                        ? orderedQuery.ThenByDescending(builder.SortOptions[i].Expression)
                        : orderedQuery.ThenBy(builder.SortOptions[i].Expression);
                }

                query = orderedQuery;
            }

            return query;
        }
    }
}
