using AxGrid.Base;
using Game.Core;
using UnityEngine;

namespace Game.Game
{
    public class CoinSpawner : MonoBehaviourExt
    {
        [Header("Limits")] [SerializeField] private int startCount = 100;
        [SerializeField] private int maxSimultaneous = 1000;

        [Header("Refs")] [SerializeField] private Coin coinPrefab;
        [SerializeField] private AreaBounds area;
        [SerializeField] private ParticleSystem spawnFxPrefab;

        private ObjectPool<Coin> _pool;
        private Transform _root;
        private Transform _fxRoot;
        private bool _initialDone;

        [OnAwake]
        public void Init()
        {
            _root = new GameObject("Coins").transform;
            _root.SetParent(transform, false);
            _fxRoot = new GameObject("FX").transform;
            _fxRoot.SetParent(transform, false);
            _pool = new ObjectPool<Coin>(coinPrefab, startCount, _root);
        }

        [OnStart]
        public void SpawnInitial()
        {
            for (int i = 0; i < startCount; i++)
                SpawnOne(false);
            
            _initialDone = true;
        }

        [OnRefresh(0.2f)]
        public void SpawnTick()
        {
            if (Coin.Active.Count < maxSimultaneous)
                SpawnOne(true);
        }

        void SpawnOne(bool withFx)
        {
            var pos = area ? area.RandomPoint() : transform.position;
            var c = _pool.Get();
            c.transform.position = pos;
            c.Setup(this);

            if (withFx && spawnFxPrefab)
            {
                var fx = Instantiate(spawnFxPrefab, pos, Quaternion.identity, _fxRoot);
                fx.Play();
                Destroy(fx.gameObject, 1.5f);
            }
        }

        public void Despawn(Coin coin) => _pool.Release(coin);
    }
}