using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace AdmitOne.Persistence
{
    public class Session : ISession
    {
        private DbContext _context;

        internal Session(DbContext context)
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
            return _context.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)

                // Filter by...
                .Where(p =>

                    // Restricting to generically typed properties
                     p.PropertyType.IsGenericType &&

                        // Checking if the property is an IDbSet<> itself, or...
                        ((p.PropertyType.IsInterface && p.PropertyType.GetGenericTypeDefinition() == typeof(IDbSet<>)) ||

                        // Checking if the property implements IDbSet<>
                        (p.PropertyType.GetInterfaces()
                            .Where(m => m.IsGenericType)
                            .Select(m => m.GetGenericTypeDefinition())
                            .Any(m => m == typeof(IDbSet<>)))))

                // Then dump all of the resulting type arguments into one Enumerable
                .SelectMany(x => x.PropertyType.GetGenericArguments());
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public IObservable<IEnumerable<T>> FetchResults<T>(IQuery<T> query)
        {
            throw new NotImplementedException();
        }
    }
}
