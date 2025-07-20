using AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.Includer;
using AirSoft.Exceptions;
using System.Linq.Expressions;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.Includer
{
    /// <summary>
    /// Provides continuation methods for specifying nested include paths after an initial include.
    /// </summary>
    /// <typeparam name="TEntity">The root entity type.</typeparam>
    /// <typeparam name="TPreviousProperty">The type of the previously included property.</typeparam>
    public class ThenIncluder<TEntity, TPreviousProperty> : Includer<TEntity>, IThenIncluder<TEntity, TPreviousProperty> where TEntity : IEntityBase
    {
        /// <summary>
        /// Reference to the root includer instance that started the include chain.
        /// Maintains the complete collection of all include paths.
        /// </summary>
        private readonly IIncluder<TEntity> _rootIncluder;

        /// <summary>
        /// The current include path up to this point in the chain,
        /// represented as dot-separated property names.
        /// </summary>
        private readonly string _currentPath;

        /// <summary>
        /// internal constructor to restrict usage outside the library.
        /// </summary>
        internal ThenIncluder(IIncluder<TEntity> rootIncluder, string currentPath)
        {
            _rootIncluder = rootIncluder;
            _currentPath = currentPath;
        }

        /// <summary>
        /// Specifies a nested related entity to include from the previously included property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the related entity to include.</typeparam>
        /// <param name="expression">A lambda expression representing the navigation property.</param>
        /// <exception cref="InvalidArgumentException">Thrown when expression is null or invalid</exception>
        public IThenIncluder<TEntity, TProperty> WithThenInclude<TProperty>(Expression<Func<TPreviousProperty, TProperty>> expression) where TProperty : IEntityBase
        {
            _ = expression ?? throw new InvalidArgumentException($"Using {nameof(WithThenInclude)} without expression is not allowed");
            string nestedPath = GetNestedPathFromExpression(expression);
            string fullPath = $"{_currentPath}.{nestedPath}";

            _rootIncluder.Paths.Add(fullPath);

            return new ThenIncluder<TEntity, TProperty>(_rootIncluder, fullPath);
        }

        /// <summary>
        /// Specifies a nested collection of related entities to include from the previously included property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the related entities in the collection.</typeparam>
        /// <param name="expression">A lambda expression representing the collection navigation property.</param>
        /// <exception cref="InvalidArgumentException">Thrown when expression is null or invalid</exception>
        public IThenIncluder<TEntity, TProperty> WithThenInclude<TProperty>(Expression<Func<TPreviousProperty, IEnumerable<TProperty>>> expression) where TProperty : IEntityBase
        {
            _ = expression ?? throw new InvalidArgumentException($"Using {nameof(WithThenInclude)} without expression is not allowed");
            string nestedPath = GetNestedPathFromExpression(expression);
            string fullPath = $"{_currentPath}.{nestedPath}";

            _rootIncluder.Paths.Add(fullPath);

            return new ThenIncluder<TEntity, TProperty>(_rootIncluder, fullPath);
        }

        /// <summary>
        /// Specifies an additional related entity to include from the root entity.
        /// </summary>
        /// <typeparam name="TProperty">The type of the related entity to include.</typeparam>
        /// <param name="expression">Lambda expression specifying the navigation property from root entity.</param>
        public new IThenIncluder<TEntity, TProperty> WithInclude<TProperty>(Expression<Func<TEntity, TProperty>> expression) where TProperty : IEntityBase
            => _rootIncluder.WithInclude(expression);

        /// <summary>
        /// Specifies an additional collection of related entities to include from the root entity.
        /// </summary>
        /// <typeparam name="TProperty">The type of entities in the collection to include.</typeparam>
        /// <param name="expression">Lambda expression specifying the collection navigation property from root entity.</param>
        public new IThenIncluder<TEntity, TProperty> WithInclude<TProperty>(Expression<Func<TEntity, IEnumerable<TProperty>>> expression) where TProperty : IEntityBase
            => _rootIncluder.WithInclude(expression);
    }
}
