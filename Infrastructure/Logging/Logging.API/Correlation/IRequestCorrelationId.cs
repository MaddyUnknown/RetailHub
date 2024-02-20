namespace Logging.API.Correlation
{
    public interface IRequestCorrelationId
    {
        string Id { get; set; }
    }
}