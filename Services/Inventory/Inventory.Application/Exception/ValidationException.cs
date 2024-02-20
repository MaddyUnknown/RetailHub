
namespace Inventory.Application.Exception
{
    public class ValidationException : System.Exception
    {
        public List<string> _errorList;

        public ValidationException(string message) : base(message)
        {
            _errorList = new List<string>();
        }

        public ValidationException(List<string> errorList, string message) : base(message)
        {
            _errorList = errorList ?? new List<string>();
        }

        public List<string> ErrorList
        {
            get
            {
                return _errorList;
            }
        }
    }
}
