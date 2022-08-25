using Scripts.Data;
using Scripts.Infrastructure.Services.PersistentProgress;

namespace Scripts.Infrastructure.States
{
    internal class LoadProgressState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IPersistentProgressService _progressService;

        public LoadProgressState(GameStateMachine stateMachine, IPersistentProgressService progressService)
        {
            _stateMachine = stateMachine;
            _progressService = progressService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            string levelName = _progressService.Progress.WorldData.PositionOnLevel.Level;
            _stateMachine.Enter<LoadLeveStatr, string>(levelName);
        }


        public void Exit()
        {
        }

        private void LoadProgressOrInitNew()
        {
            _progressService.Progress = /*_saveLoadServise.LoadProgress() ??*/ NewProgress();
            _ = _progressService.Progress;
        }

        private PlayerProgress NewProgress()
        {
            PlayerProgress playerProgress = new PlayerProgress("Main");

            // playerProgress.HeroState.ResetHP();

            return playerProgress;
        }
    }
}