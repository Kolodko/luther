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
        public bool CanStop  => State == GameState.Collecting;

        public Action<GameState> OnStateChanged;

        [OnAwake]
        public void Init()
        {
            SetState(GameState.Idle);
            UpdateButtonsModel();
        }

        [OnStart]
        public void Hook()
        {
            Model?.EventManager.AddAction("OnStartClick", OnStartClick);
            Model?.EventManager.AddAction("OnStopClick",  OnStopClick);
        }

        [OnDestroy]
        public void Unhook()
        {
            Model?.EventManager.RemoveAction("OnStartClick", OnStartClick);
            Model?.EventManager.RemoveAction("OnStopClick",  OnStopClick);
        }

        private void OnStartClick() => CmdStart();
        private void OnStopClick()  => CmdStop();

        private void SetState(GameState newState)
        {
            if (State == newState)
                return;
            
            if (State == GameState.Collecting && player)
                player.StopCollect();

            State = newState;
            
            if (State == GameState.Collecting && player)
                player.StartCollect();

            OnStateChanged?.Invoke(State);
            UpdateButtonsModel();
        }

        private void UpdateButtonsModel()
        {
            if (Model == null)
                return;
            
            Model["BtnStartEnable"] = CanStart;
            Model["BtnStopEnable"]  = CanStop;
            
            Model.EventManager.Invoke("OnBtnStartEnableChanged");
            Model.EventManager.Invoke("OnBtnStopEnableChanged");
        }

        public void CmdStart() => HandleSignal(GameSignal.Start);
        public void CmdStop()  => HandleSignal(GameSignal.Stop);

        public void HandleSignal(GameSignal sig)
        {
            if (sig == GameSignal.Start && State == GameState.Idle)
                SetState(GameState.Collecting);
            else if (sig == GameSignal.Stop && State == GameState.Collecting)
                SetState(GameState.Idle);
        }
    }
}
