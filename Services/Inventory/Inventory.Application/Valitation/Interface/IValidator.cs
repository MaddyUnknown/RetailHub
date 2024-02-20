using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Valitation.Interface
{
    public interface IValidator<T>
    {
        Task<ValidationResult> IsValid(T obj);
    }
}
