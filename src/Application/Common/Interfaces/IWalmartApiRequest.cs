namespace WOF.Application.Common.Interfaces;

public interface IWalmartApiRequest
{
    Task<T> GetResponse<T>();
}
