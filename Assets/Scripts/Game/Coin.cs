using System.Collections.Generic;
using UnityEngine;

namespace Game.Game
{
    [RequireComponent(typeof(Collider))]
    public class Coin : MonoBehaviour
    {
        [SerializeField] private AudioClip pickupSfx;

        public static readonly List<Coin> Active = new List<Coin>(1024);
        private Collider _col;
        private Transform _tf;
        private CoinSpawner _spawner;
        
        private void Awake()
        {
            _col = GetComponent<Collider>();
            _col.isTrigger = true;
            _tf = transform;
        }
        
        internal void Setup(CoinSpawner spawner)
        {
            _spawner = spawner;
        }
        
        private void OnEnable()
        {
            if (!Active.Contains(this)) 
                Active.Add(this);
        }

        private void OnDisable()
        {
            Active.Remove(this);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) 
                return;
            
            Collect();
        }

        public void Collect()
        {
            if (pickupSfx)
                AudioSource.PlayClipAtPoint(pickupSfx, _tf.position, 0.8f);

            _spawner.Despawn(this);
        }
    }
}