using Hanssens.Net;
using MVC.Contracts;

namespace MVC.Services;

public class LocalStorageService : ILocalStorageService
{
    private LocalStorage storage;

	public LocalStorageService()
	{
		var config = new LocalStorageConfiguration()
		{
			AutoLoad = true,
			AutoSave = true,
			Filename = "CleanAPI"
		};

		storage = new LocalStorage(config);
	}

	public void ClearStorage(List<string> keys)
	{
		//for (int i = 0; i < keys.Count; i++)
		//{
		//	keys.Remove(keys[i]);
		//}
		storage.Clear();
    }

	public bool Exists(string key)
	{
		return storage.Exists(key);
	}

	public T GetStorageValue<T>(string key)
	{
		return storage.Get<T>(key);
	}

	public void SetStorageValue<T>(string key, T value)
	{
		storage.Store(key, value);
		storage.Persist();
	}
}
