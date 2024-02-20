namespace Inventory.API.Response
{
    public class ResponseWrapper<T>
    {
        public bool IsSuccess { get; set; }
        public string? Description { get; set; }
        public T? Result { get; set; }

        public static ResponseWrapper<T> CreateResponse(bool isSuccess, string? description, T? result)
        {
            return new ResponseWrapper<T>
            {
                IsSuccess = isSuccess,
                Description = description,
                Result = result
            };
        }
    }
}
