using AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.Includer;
using AirSoft.EntityFrameworkCore.Abstractions.Entities;
using AirSoft.Exceptions;
using System.ComponentModel;
using System.Linq.Expressions;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.Includer
{
    /// <summary>
    /// Provides a fluent interface for specifying entity include paths for eager loading related data.
    /// </summary>
    /// <typeparam name="TEntity">The root entity type to include relations for, must inherit from <see cref="EntityBase"/>.</typeparam>
    public class Includer<TEntity> : IIncluder<TEntity> where TEntity : EntityBase
    {
        /// <summary>
        /// internal constructor to restrict usage outside the library.
        /// </summary>
        internal Includer()
        {
        }

        /// <summary>
        /// Gets the collection of include paths that have been specified.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public HashSet<string> Paths { get; } = [];

        /// <summary>
        /// Specifies a related entity to include in the query results.
        /// </summary>
        /// <typeparam name="TProperty">The type of the related entity to include.</typeparam>
        /// <param name="expression">A lambda expression representing the navigation property.</param>
        /// <exception cref="InvalidArgumentException">Thrown when expression is null or invalid</exception>
        public IThenIncluder<TEntity, TProperty> WithInclude<TProperty>(Expression<Func<TEntity, TProperty>> expression) where TProperty : EntityBase
        {
            _ = expression ?? throw new InvalidArgumentException($"Using {nameof(WithInclude)} without expression is not allowed");
            string path = GetPathFromExpression(expression);
            Paths.Add(path);

            return new ThenIncluder<TEntity, TProperty>(this, path);
        }

        /// <summary>
        /// Specifies a collection of related entities to include in the query results.
        /// </summary>
        /// <typeparam name="TProperty">The type of the related entities in the collection.</typeparam>
        /// <param name="expression">A lambda expression representing the collection navigation property.</param>
        /// <exception cref="InvalidArgumentException">Thrown when expression is null or invalid</exception>
        public IThenIncluder<TEntity, TProperty> WithInclude<TProperty>(Expression<Func<TEntity, IEnumerable<TProperty>>> expression) where TProperty : EntityBase
        {
            _ = expression ?? throw new InvalidArgumentException($"Using {nameof(WithInclude)} without expression is not allowed");
            string path = GetPathFromExpression(expression);
            Paths.Add(path);

            return new ThenIncluder<TEntity, TProperty>(this, path);
        }

        /// <summary>
        /// Extracts the member name from a property access expression.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property being accessed.</typeparam>
        /// <param name="expression">The property access expression to analyze.</param>
        /// <returns>The member name as string.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when expression is invalid</exception>
        protected static string GetPathFromExpression<TProperty>(Expression<Func<TEntity, TProperty>> expression)
            => GetMemberName(expression?.Body) ?? throw new InvalidArgumentException("Invalid property accessor");

        /// <summary>
        /// Extracts the member name from a nested property access expression.
        /// </summary>
        /// <typeparam name="TPreviousProperty">The type of the parent property.</typeparam>
        /// <typeparam name="TNewProperty">The type of the property being accessed.</typeparam>
        /// <param name="expression">The nested property access expression to analyze.</param>
        /// <returns>The member name as string.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when expression is invalid</exception>
        protected static string GetNestedPathFromExpression<TPreviousProperty, TNewProperty>(Expression<Func<TPreviousProperty, TNewProperty>> expression)
            => GetMemberName(expression?.Body) ?? throw new InvalidArgumentException("Invalid property accessor");

        /// <summary>
        /// Extracts the member name from an expression if it represents a member access.
        /// </summary>
        /// <param name="expression">The expression to analyze.</param>
        /// <returns>The name of the member being accessed, or null if the expression is not a member access.</returns>
        private static string? GetMemberName(Expression? expression)
            => (expression as MemberExpression)?.Member.Name;
    }
}
