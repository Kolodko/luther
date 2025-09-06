using Game.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIStartStopPresenter : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button stopButton;
        [SerializeField] private GameFsmController fsm;
        
        private void Awake()
        {
            if (fsm) 
                fsm.OnStateChanged += OnState;
        }

        private void OnDestroy()
        {
            if (fsm) 
                fsm.OnStateChanged -= OnState;
        }

        private void Start()
        {
            Refresh();
        }
        
        private void OnState(GameState _)
        {
            Refresh();
        }
        
        private void Refresh()
        {
            if (!fsm) 
                return;
            
            if (startButton) 
                startButton.interactable = fsm.CanStart;
            
            if (stopButton) 
                stopButton.interactable = fsm.CanStop;
        }

        public void CmdStart() => fsm?.CmdStart();
        public void CmdStop() => fsm?.CmdStop();
    }
}