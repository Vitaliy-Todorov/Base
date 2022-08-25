using Scripts.Infrastructure.Services.Factory;
using Scrips.Infrastructure;
using Scripts.CameraLogic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Scrips.Logic;

namespace Scripts.Infrastructure.States
{
    internal class LoadLeveStatr : IPlaylaodedState<string>
    {

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _curtain;
        private readonly IGameFactory _gameFactory;

        public LoadLeveStatr(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain, IGameFactory gameFactory)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _gameFactory = gameFactory;
        }

        public void Enter(string sceneName)
        {
            _curtain.Show();
            _gameFactory.Cleanup();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() =>
            _curtain.Hide();

        private async void OnLoaded()
        {
            await InitGameWord();

            _stateMachine.Enter<GameLoopState>();
        }

        private async Task InitGameWord()
        {
            GameObject character = await CreateCharacterAsync(/*LevelStaticData()*/);
            await CreateHudAsync(character);

            CameraFllow(character);
        }

        private async Task<GameObject> CreateCharacterAsync(/*LevelStaticData levelStaticData*/) => 
            await _gameFactory.CreateCharacterAsync(at: new Vector3() /*levelStaticData.InitialHeroPosition*/);

        private async Task<GameObject> CreateHudAsync(GameObject character)
        {
            GameObject hud = await _gameFactory.CreateHudAsync();
            /*HeroHealth heroHealth = character.GetComponentInChildren<HeroHealth>();
            hud.GetComponentInChildren<ActorUI>()
                .Construct(heroHealth);*/
            return hud;
        }

        /*private LevelStaticData LevelStaticData()
        {
            string sceneKey = SceneManager.GetActiveScene().name;
            return _staticData.ForLevel(sceneKey);
        }*/

        private static void CameraFllow(GameObject character)
        {
            Camera.main
                .GetComponent<CameraFollow>()
                .Follow(character);
        }
    }
}