using Scripts.Infrastructure.Services;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Scripts.Infrastructure.Services.AssetManagement
{
    public interface IAssetProvider : IService
    {
        void Initialize();
        Task<GameObject> Instantiate(string path);
        Task<GameObject> Instantiate(string path, Vector3 at);
        Task<T> Load<T>(AssetReference assetReference) where T : class;
        void CleanUp();
        Task<T> Load<T>(string address) where T : class;
    }
}