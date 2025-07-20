using System.ComponentModel;
using System.Linq.Expressions;
using AirSoft.Exceptions;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.Includer
{
    /// <summary>
    /// Provides a fluent interface for specifying entity include paths for eager loading related data.
    /// </summary>
    /// <typeparam name="TEntity">The root entity type to include relations for, must inherit from IEntityBase.</typeparam>
    public interface IIncluder<TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Gets the collection of include paths that have been specified.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public HashSet<string> Paths { get; }

        /// <summary>
        /// Specifies a related entity to include in the query results.
        /// </summary>
        /// <typeparam name="TProperty">The type of the related entity to include.</typeparam>
        /// <param name="expression">A lambda expression representing the navigation property.</param>
        /// <exception cref="InvalidArgumentException">Thrown when expression is null or invalid</exception>
        IThenIncluder<TEntity, TProperty> WithInclude<TProperty>(Expression<Func<TEntity, TProperty>> expression)
            where TProperty : IEntityBase;

        /// <summary>
        /// Specifies a collection of related entities to include in the query results.
        /// </summary>
        /// <typeparam name="TProperty">The type of the related entities in the collection.</typeparam>
        /// <param name="expression">A lambda expression representing the collection navigation property.</param>
        /// <exception cref="InvalidArgumentException">Thrown when expression is null or invalid</exception>
        IThenIncluder<TEntity, TProperty> WithInclude<TProperty>(Expression<Func<TEntity, IEnumerable<TProperty>>> expression)
            where TProperty : IEntityBase;
    }
}
