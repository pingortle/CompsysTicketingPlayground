using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace AdmitOne.Persistence
{
    public class Repository<T> : IRepository where T : DbContext
    {
        private DbContext _context;

        internal Repository(T context)
        {
            _context = context;
        }

        public IStore<TStore> GetStoreOf<TStore>()
        {
            return new Store<TStore>(_context);
        }

        public IEnumerable<Type> GetAvailableTypes()
        {
            // Get all of the public, instance properties
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)

                // Filter by...
                .Where(x =>
                {
                    // Restricting to generically typed properties
                    return x.PropertyType.IsGenericType &&
                        (
                        // Checking if the property is an IDbSet<> itself, or...
                            (x.PropertyType.IsInterface && x.PropertyType.GetGenericTypeDefinition() == typeof(IDbSet<>)) ||
                        // Checking if the property implements IDbSet<>
                            (x.PropertyType.GetInterfaces().Where(m => m.IsGenericType).Select(m => m.GetGenericTypeDefinition()).Any(m => m == typeof(IDbSet<>)))
                        );
                })

                // Then dump all of the resulting type arguments into one Enumerable
                .SelectMany(x => x.PropertyType.GetGenericArguments());
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }
    }
}
