using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Utility
{
    internal static class ReflectionUtil
    {
        internal static void SetPropertyValue<T, U>(T objectVal, string propertyName, U value)
        {
            PropertyInfo? propertyInfo = typeof(T).GetProperty(propertyName);

            if(propertyInfo == null)
            {
                throw new InvalidOperationException($"No property named '{propertyName}' in type '{typeof(T)}'");
            }
            else if(!propertyInfo.PropertyType.Equals(typeof(U)))
            {
                throw new InvalidCastException($"'{propertyName}' not of type '{typeof(U)}'");
            }
            else
            {
                propertyInfo.SetValue(objectVal, value);
            }
        }
    }
}
