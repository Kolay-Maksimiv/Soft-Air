using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoftAir.Data.Domain;
using SoftAir.Data.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace SoftAir.Data.Extensions
{
    public static class DbContextExtensions
    {
        #region Fields

        private static string _databaseName;
        private static readonly ConcurrentDictionary<string, string> TableNames = new();
        private static readonly ConcurrentDictionary<string, IEnumerable<(string, int?)>> ColumnsMaxLength = new();
        private static readonly ConcurrentDictionary<string, IEnumerable<(string, decimal?)>> DecimalColumnsMaxValue = new();

        #endregion

        #region Utilities

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AppConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("SoftAir.Data")));

            return services;
        }

        /// <summary>
        /// Loads a copy of the entity using the passed function
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="context">Database context</param>
        /// <param name="entity">Entity</param>
        /// <param name="getValuesFunction">Function to get the values of the tracked entity</param>
        /// <returns>Copy of the passed entity</returns>
        private static TEntity LoadEntityCopy<TEntity>(IDbContext context, TEntity entity, Func<EntityEntry<TEntity>, PropertyValues> getValuesFunction)
            where TEntity : Entity
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            //try to get the EF database context
            if (context is not DbContext dbContext)
                throw new InvalidOperationException("Context does not support operation");

            //try to get the entity tracking object
            var entityEntry = dbContext.ChangeTracker.Entries<TEntity>().FirstOrDefault(entry => entry.Entity == entity);
            if (entityEntry == null)
                return null;

            //get a copy of the entity
            var entityCopy = getValuesFunction(entityEntry)?.ToObject() as TEntity;

            return entityCopy;
        }

        /// <summary>
        /// Get SQL commands from the script
        /// </summary>
        /// <param name="sql">SQL script</param>
        /// <returns>List of commands</returns>
        private static IList<string> GetCommandsFromScript(string sql)
        {
            var commands = new List<string>();

            //origin from the Microsoft.EntityFrameworkCore.Migrations.SqlServerMigrationsSqlGenerator.Generate method
            sql = Regex.Replace(sql, @"\\\r?\n", string.Empty);
            var batches = Regex.Split(sql, @"^\s*(GO[ \t]+[0-9]+|GO)(?:\s+|$)", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            for (var i = 0; i < batches.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(batches[i]) || batches[i].StartsWith("GO", StringComparison.OrdinalIgnoreCase))
                    continue;

                var count = 1;
                if (i != batches.Length - 1 && batches[i + 1].StartsWith("GO", StringComparison.OrdinalIgnoreCase))
                {
                    var match = Regex.Match(batches[i + 1], "([0-9]+)");
                    if (match.Success)
                        count = int.Parse(match.Value);
                }

                var builder = new StringBuilder();
                for (var j = 0; j < count; j++)
                {
                    builder.Append(batches[i]);
                    if (i == batches.Length - 1)
                        builder.AppendLine();
                }

                commands.Add(builder.ToString());
            }

            return commands;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the original copy of the entity
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="context">Database context</param>
        /// <param name="entity">Entity</param>
        /// <returns>Copy of the passed entity</returns>
        public static TEntity LoadOriginalCopy<TEntity>(this IDbContext context, TEntity entity) where TEntity : Entity
        {
            return LoadEntityCopy(context, entity, entityEntry => entityEntry.OriginalValues);
        }

        /// <summary>
        /// Loads the database copy of the entity
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="context">Database context</param>
        /// <param name="entity">Entity</param>
        /// <returns>Copy of the passed entity</returns>
        public static TEntity LoadDatabaseCopy<TEntity>(this IDbContext context, TEntity entity) where TEntity : Entity
        {
            return LoadEntityCopy(context, entity, entityEntry => entityEntry.GetDatabaseValues());
        }

        /// <summary>
        /// Drop a plugin table
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="tableName">Table name</param>
        public static void DropPluginTable(this IDbContext context, string tableName)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            //drop the table
            var dbScript = $"IF OBJECT_ID('{tableName}', 'U') IS NOT NULL DROP TABLE [{tableName}]";
            context.ExecuteSqlCommand(dbScript);
            context.SaveChanges();
        }

        /// <summary>
        /// Get table name of entity
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="context">Database context</param>
        /// <returns>Table name</returns>
        public static string GetTableName<TEntity>(this IDbContext context) where TEntity : Entity
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            //try to get the EF database context
            if (context is not DbContext dbContext)
                throw new InvalidOperationException("Context does not support operation");

            var entityTypeFullName = typeof(TEntity).FullName;
            if (!TableNames.ContainsKey(entityTypeFullName))
            {
                //get entity type
                var entityType = dbContext.Model.FindEntityType(typeof(TEntity));

                //get the name of the table to which the entity type is mapped
                TableNames.TryAdd(entityTypeFullName, entityType.GetTableName());
            }

            TableNames.TryGetValue(entityTypeFullName, out var tableName);

            return tableName;
        }

        /// <summary>
        /// Gets the maximum lengths of data that is allowed for the entity properties
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="context">Database context</param>
        /// <returns>Collection of name - max length pairs</returns>
        public static IEnumerable<(string Name, int? MaxLength)> GetColumnsMaxLength<TEntity>(this IDbContext context) where TEntity : Entity
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            //try to get the EF database context
            if (context is not DbContext dbContext)
                throw new InvalidOperationException("Context does not support operation");

            var entityTypeFullName = typeof(TEntity).FullName;
            if (!ColumnsMaxLength.ContainsKey(entityTypeFullName))
            {
                //get entity type
                var entityType = dbContext.Model.FindEntityType(typeof(TEntity));

                //get property name - max length pairs
                ColumnsMaxLength.TryAdd(entityTypeFullName,
                    entityType.GetProperties().Select(property => (property.Name, property.GetMaxLength())));
            }

            ColumnsMaxLength.TryGetValue(entityTypeFullName, out var result);

            return result;
        }

        /// <summary>
        /// Get maximum decimal values
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="context">Database context</param>
        /// <returns>Collection of name - max decimal value pairs</returns>
        public static IEnumerable<(string Name, decimal? MaxValue)> GetDecimalColumnsMaxValue<TEntity>(this IDbContext context)
            where TEntity : Entity
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            //try to get the EF database context
            if (context is not DbContext dbContext)
                throw new InvalidOperationException("Context does not support operation");

            var entityTypeFullName = typeof(TEntity).FullName;
            if (!DecimalColumnsMaxValue.ContainsKey(entityTypeFullName))
            {
                //get entity type
                var entityType = dbContext.Model.FindEntityType(typeof(TEntity));

                //get entity decimal properties
                var properties = entityType.GetProperties().Where(property => property.ClrType == typeof(decimal));

                //return property name - max decimal value pairs
                DecimalColumnsMaxValue.TryAdd(entityTypeFullName, properties.Select(property =>
                {
                    var mapping = new RelationalTypeMappingInfo(property);
                    if (!mapping.Precision.HasValue || !mapping.Scale.HasValue)
                        return (property.Name, null);

                    return (property.Name, new decimal?((decimal)Math.Pow(10, mapping.Precision.Value - mapping.Scale.Value)));
                }));
            }

            DecimalColumnsMaxValue.TryGetValue(entityTypeFullName, out var result);

            return result;
        }

        /// <summary>
        /// Get database name
        /// </summary>
        /// <param name="context">Database context</param>
        /// <returns>Database name</returns>
        public static string DbName(this IDbContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            //try to get the EF database context
            if (context is not DbContext dbContext)
                throw new InvalidOperationException("Context does not support operation");

            if (!string.IsNullOrEmpty(_databaseName))
                return _databaseName;

            //get database connection
            var dbConnection = dbContext.Database.GetDbConnection();

            //return the database name
            _databaseName = dbConnection.Database;

            return _databaseName;
        }

        /// <summary>
        /// Execute commands from the SQL script against the context database
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="sql">SQL script</param>
        public static void ExecuteSqlScript(this IDbContext context, string sql)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var sqlCommands = GetCommandsFromScript(sql);
            foreach (var command in sqlCommands)
                context.ExecuteSqlCommand(command);
        }

        /// <summary>
        /// Execute commands from a file with SQL script against the context database
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="filePath">Path to the file</param>
        public static void ExecuteSqlScriptFromFile(this IDbContext context, string filePath)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (!File.Exists(filePath))
                return;

            context.ExecuteSqlScript(File.ReadAllText(filePath));
        }

        /// <summary>Returns distinct elements from a sequence by using the default equality comparer to compare values.</summary>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains distinct elements from the source sequence.</returns>
        /// <param name="source">The sequence to remove duplicate elements from.</param>
        /// <param name="keySelector"> key if func</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="TKey">The type of the elements of <paramref name="source" />.</typeparam>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        #endregion
    }
}
