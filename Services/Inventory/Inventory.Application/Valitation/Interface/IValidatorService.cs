namespace Inventory.Application.Valitation.Interface
{
    public interface IValidatorService
    {
        Task<ValidationResult> Validate<T>(T obj);
    }
}
