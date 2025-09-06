using System;
using AxGrid.Base;
using Game.Core;
using Game.Player;
using UnityEngine;

namespace Game.Game
{
    public class GameFsmController : MonoBehaviourExt
    {
        public GameState State { get; private set; } = GameState.Boot;

        [SerializeField] private PlayerAutoCollector player;

        public bool CanStart => State != GameState.Collecting;
        public bool CanStop => State == GameState.Collecting;

        public Action<GameState> OnStateChanged;

        [OnAwake]
        public void Init()
        {
            SetState(GameState.Idle);
        }

        private void SetState(GameState newState)
        {
            if (State == newState)
                return;

            switch (State)
            {
                case GameState.Collecting:
                    if (player) player.StopCollect();
                    break;
            }

            State = newState;

            switch (State)
            {
                case GameState.Collecting:
                    if (player) player.StartCollect();
                    break;
            }

            OnStateChanged?.Invoke(State);
        }
        
        public void CmdStart() => HandleSignal(GameSignal.Start);
        public void CmdStop() => HandleSignal(GameSignal.Stop);

        public void HandleSignal(GameSignal sig)
        {
            if (sig == GameSignal.Start && State == GameState.Idle) SetState(GameState.Collecting);
            else if (sig == GameSignal.Stop && State == GameState.Collecting) SetState(GameState.Idle);
        }
    }
}
