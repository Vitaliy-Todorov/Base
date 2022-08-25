using Scripts.Infrastructure.Services.AssetManagement;
using Scripts.Infrastructure.Services.Factory;
using Scripts.Infrastructure.Services;
using Scrips.Infrastructure;
using System;
using UnityEngine;
using Scripts.Infrastructure.Services.InputService;
using Scripts.Infrastructure.Services.PersistentProgress;

namespace Scripts.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string _intial = "Initial";
        private const string _main = "Main";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private AllServices _services;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;

            RegisterServices();
        }

        public void Enter() => 
            _sceneLoader.Load(_intial, onLoadwr: EnterLoadLevel);

        private void EnterLoadLevel() =>
            _stateMachine.Enter<LoadProgressState>();

        private void RegisterServices()
        {
            _services.RegisterSingle<IGameStateMachine>(_stateMachine);

            _services.RegisterSingle<IInputService>(InputService());
            _services.RegisterSingle<IAssetProvider>(AssetProvider());
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());

            _services.RegisterSingle<IGameFactory>(GameFactory());
        }

        private static IAssetProvider AssetProvider()
        {
            IAssetProvider assetProvider = new AssetProvider();
            assetProvider.Initialize();
            return assetProvider;
        }

        public void Exit()
        {
        }

        private static IInputService InputService()
        {
            if (Application.isEditor)
                return new KeyboardMouseInputServuce();
            else
                throw new Exception("InputServices == null");
        }

        private GameFactory GameFactory()
        {
            IAssetProvider assetProvider = _services.Single<IAssetProvider>();
            IInputService inputService = _services.Single<IInputService>();

            return new GameFactory(assetProvider, inputService);
        }
    }
}