using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Connections.Databases
{
    internal static class DataRecordExtensions
    {
        internal static T Convert<T>(this IDataRecord dataRecord, bool handleUnknowProperty = false)
            where T : new()
        {
            Type type = typeof(T);
            T result = new T();

            foreach(PropertyInfo propertyInfo in type.GetProperties())
            {
                try
                {
                    object value = dataRecord[propertyInfo.Name];

                    if (propertyInfo.SetMethod is null)
                        throw new InvalidOperationException("Le mutateur doit exister et être de niveau d'accès 'public'.");

                    if(propertyInfo.PropertyType == typeof(DateOnly))
                    {
                        propertyInfo.SetMethod?.Invoke(result, new object[] { DateOnly.FromDateTime((DateTime)value) });
                    }
                    else
                    {
                        propertyInfo.SetMethod?.Invoke(result, new object[] { value });
                    }
                    
                }
                catch (IndexOutOfRangeException)
                {
                    if (handleUnknowProperty)
                        throw new InvalidOperationException($"La propriété '{propertyInfo.Name}' n'existe pas dans le résultat de la requête.");
                }
                catch (ArgumentException ex)
                {
                    throw new InvalidOperationException($"Erreur avec la propriété '{propertyInfo.Name}' voir (InnerException)", ex);
                }
            }

            return result;
        }
    }
}
