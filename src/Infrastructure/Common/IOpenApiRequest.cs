namespace WOF.Application.Common.Interfaces;

public interface IOpenApiRequest
{
    Task<T> GetResponse<T>(string prompt);
}
