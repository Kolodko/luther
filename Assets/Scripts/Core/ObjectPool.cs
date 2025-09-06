using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class ObjectPool<T> where T : Component
    {
        private readonly T _prefab;
        private readonly Transform _root;
        private readonly Stack<T> _stack = new Stack<T>();
        
        public ObjectPool(T prefab, int preload, Transform root)
        {
            _prefab = prefab;
            _root = root;
            
            for (int i = 0; i < preload; i++)
            {
                var inst = Object.Instantiate(_prefab, _root);
                inst.gameObject.SetActive(false);
                _stack.Push(inst);
            }
        }
        
        public T Get()
        {
            var inst = _stack.Count > 0 ? _stack.Pop() : Object.Instantiate(_prefab, _root);
            inst.gameObject.SetActive(true);
            
            return inst;
        }
        
        public void Release(T inst)
        {
            inst.gameObject.SetActive(false);
            _stack.Push(inst);
        }
    }
}
