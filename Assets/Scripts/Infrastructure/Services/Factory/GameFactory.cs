using Scripts.Infrastructure.Services.AssetManagement;
using UnityEngine;
using System.Threading.Tasks;
using Assets.Scripts.Characters.Player;
using Scripts.Infrastructure.Services.InputService;

namespace Scripts.Infrastructure.Services.Factory
{
    public class GameFactory : IGameFactory
    {
        private IAssetProvider _assetProvider;
        private IInputService _inputService;

        public GameFactory(IAssetProvider assetProvider, IInputService inputService)
        {
            _assetProvider = assetProvider;
            _inputService = inputService;
        }

        public async Task<GameObject> CreateHudAsync()
        {
            GameObject hud = await InstantiateRegisteredAsync(AssetAddress.HudPath);

            return hud;
        }

        public async Task<GameObject> CreateCharacterAsync(Vector3 at)
        {
            GameObject PlayerGO = await InstantiateRegisteredAsync(AssetAddress.PlayerPath, at);
            PlayerMove playerMove = PlayerGO.GetComponent<PlayerMove>();
            playerMove.Construct(10, _inputService);

            return PlayerGO;
        }

        public void Cleanup()
        {
            _assetProvider.CleanUp();
        }

        private GameObject InstantiateRegistered(GameObject prefab)
        {
            GameObject gameObject = Object.Instantiate(prefab);

            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
        {
            GameObject gameObject = Object.Instantiate(prefab, at, Quaternion.identity);

            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
        {
            GameObject gameObject = await _assetProvider.Instantiate(prefabPath);

            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 at)
        {
            GameObject gameObject = await _assetProvider.Instantiate(prefabPath, at);

            return gameObject;
        }
    }
}