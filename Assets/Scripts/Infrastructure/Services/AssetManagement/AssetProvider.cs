using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Scripts.Infrastructure.Services.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private Dictionary<string, AsyncOperationHandle> _completedCache = new Dictionary<string, AsyncOperationHandle>();
        private Dictionary<string, List<AsyncOperationHandle>> _handles = new Dictionary<string, List<AsyncOperationHandle>>();

        public void Initialize() => 
            Addressables.InitializeAsync();

        public async Task<T> Load<T>(AssetReference assetReference) where T : class
        {
            if (_completedCache.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            AsyncOperationHandle<T> handle = Addressables.LoadAsset<T>(assetReference);

            return await RunWithCacheOnComplete(handle, assetReference.AssetGUID);
        }

        public async Task<T> Load<T>(string address) where T : class
        {
            if (_completedCache.TryGetValue(address, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            AsyncOperationHandle<T> handle = Addressables.LoadAsset<T>(address);

            return await RunWithCacheOnComplete<T>(handle, address);
        }

        public Task<GameObject> Instantiate(string address) =>
            Addressables.InstantiateAsync(address).Task;

        public Task<GameObject> Instantiate(string address, Vector3 at) => 
            Addressables.InstantiateAsync(address, at, Quaternion.identity).Task;

        public void CleanUp()
        {
            foreach (List<AsyncOperationHandle> resourceHandle in _handles.Values)
                foreach (AsyncOperationHandle handle in resourceHandle)
                    Addressables.Release(handle);

            _completedCache.Clear();
            _handles.Clear();
        }

        private async Task<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
        {
            handle.Completed += h =>
            {
                _completedCache[cacheKey] = h;
            };

            Addhandle(cacheKey, handle);

            return await handle.Task;
        }

        private void Addhandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> resourceHandle))
            {
                resourceHandle = new List<AsyncOperationHandle>();
                _handles[key] = resourceHandle;
            }

            resourceHandle.Add(handle);  
        }
    }
}