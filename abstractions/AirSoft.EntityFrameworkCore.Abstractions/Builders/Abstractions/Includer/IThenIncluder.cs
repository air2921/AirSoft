using System.Linq.Expressions;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.Includer
{
    /// <summary>
    /// Provides continuation methods for specifying nested include paths after an initial include.
    /// </summary>
    /// <typeparam name="TEntity">The root entity type.</typeparam>
    /// <typeparam name="TPreviousProperty">The type of the previously included property.</typeparam>
    public interface IThenIncluder<TEntity, TPreviousProperty> : IIncluder<TEntity>
        where TEntity : IEntityBase
    {
        /// <summary>
        /// Specifies a nested related entity to include from the previously included property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the related entity to include.</typeparam>
        /// <param name="expression">A lambda expression representing the navigation property.</param>
        IThenIncluder<TEntity, TProperty> WithThenInclude<TProperty>(Expression<Func<TPreviousProperty, TProperty>> expression)
            where TProperty : IEntityBase;

        /// <summary>
        /// Specifies a nested collection of related entities to include from the previously included property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the related entities in the collection.</typeparam>
        /// <param name="expression">A lambda expression representing the collection navigation property.</param>
        IThenIncluder<TEntity, TProperty> WithThenInclude<TProperty>(Expression<Func<TPreviousProperty, IEnumerable<TProperty>>> expression)
            where TProperty : IEntityBase;
    }
}
