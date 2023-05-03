namespace WOF.Application.Common.Interfaces;

public interface IApiRequest
{
    Task<T> GetResponse<T>();
}
