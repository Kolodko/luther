using System.Collections.Generic;
using AxGrid.Base;
using Game.Game;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerAutoCollector : MonoBehaviourExt
    {
        [SerializeField] private Animator animator;
        static readonly int HashIsRunning = Animator.StringToHash("IsRunning");
        
        [SerializeField] private float moveSpeed = 3.5f;
        [SerializeField] private float stopDistance = 0.25f;

        private CharacterController _cc;
        private Transform _tf;
        private Coin _target;
        private bool _active;

        [OnAwake]
        public void Cache()
        {
            _cc = GetComponent<CharacterController>();
            _tf = transform;
        }

        public void StartCollect()
        {
            _active = true;
            _target = null;
            
            if (animator) 
                animator.SetBool(HashIsRunning, true);
        }

        public void StopCollect()
        {
            _active = false;
            _target = null;
            
            if (animator) 
                animator.SetBool(HashIsRunning, false);
        }

        [OnUpdate]
        public void Tick()
        {
            if (!_active) return;
            if (_target == null || !_target.gameObject.activeInHierarchy)
                _target = FindClosestCoin();
            if (_target == null) return;

            Vector3 dir = (_target.transform.position - _tf.position);
            dir.y = 0f;
            float dist = dir.magnitude;
            if (dist > stopDistance)
            {
                Vector3 step = dir.normalized * moveSpeed * Time.deltaTime;
                _cc.Move(step);
                _tf.forward = Vector3.Lerp(_tf.forward, dir.normalized, 10f * Time.deltaTime);
            }
        }

        private Coin FindClosestCoin()
        {
            List<Coin> list = Coin.Active;
            Coin best = null; float bestSqr = float.MaxValue; Vector3 p = _tf.position;
            for (int i = 0; i < list.Count; i++)
            {
                var c = list[i]; if (!c || !c.gameObject.activeInHierarchy) continue;
                float sqr = (c.transform.position - p).sqrMagnitude;
                if (sqr < bestSqr) { bestSqr = sqr; best = c; }
            }
            return best;
        }
    }
}
