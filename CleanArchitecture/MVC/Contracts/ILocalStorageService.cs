namespace MVC.Contracts;

public interface ILocalStorageService
{
    public void ClearStorage(List<string> keys);
    public bool Exists(string key);
    public T GetStorageValue<T> (string key);
    public void SetStorageValue<T> (string key,T value);
}
