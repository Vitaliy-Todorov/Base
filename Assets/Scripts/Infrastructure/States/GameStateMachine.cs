using Scripts.Infrastructure.Services.Factory;
using Scripts.Infrastructure.Services;
using Scrips.Infrastructure;
using Scrips.Logic;
using System;
using System.Collections.Generic;
using Scripts.Infrastructure.Services.PersistentProgress;

namespace Scripts.Infrastructure.States
{
    public class GameStateMachine : IGameStateMachine
    {
        private Dictionary<Type, IExitablState> _states;
        private IExitablState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain curtain, AllServices services)
        {
            _states = new Dictionary<Type, IExitablState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services),
                [typeof(LoadLeveStatr)] = new LoadLeveStatr(this, sceneLoader, curtain, services.Single<IGameFactory>()),
                [typeof(LoadProgressState)] = new LoadProgressState(this, services.Single<IPersistentProgressService>()),
                [typeof(GameLoopState)] = new GameLoopState(this),
            };
        }

        // : class - ��������� �� ��, ��� ����� ������ ���� ���������
        public void Enter<TState>() where TState : class, IState
        {
            TState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPlayload>(TPlayload playload) where TState : class, IPlaylaodedState<TPlayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(playload);
        }

        private TState ChangeState<TState>() where TState : class, IExitablState
        {
            // ? - ��������� �� noll ������� �������� � ��������� ���������
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TExitablState GetState<TExitablState>() where TExitablState : class, IExitablState =>
            _states[typeof(TExitablState)] as TExitablState;
    }
}