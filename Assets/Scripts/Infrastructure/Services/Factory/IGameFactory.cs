using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Infrastructure.Services.Factory
{
    public interface IGameFactory : IService
    {
        void Cleanup();
        Task<GameObject> CreateCharacterAsync(Vector3 at);
        Task<GameObject> CreateHudAsync();
    }
}