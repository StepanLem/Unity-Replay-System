using System;
using UnityEngine;

namespace StepanLem.ReplaySystem
{
    public abstract class MonoTicker : MonoBehaviour, ITicker
    {
        private int _subscriberCount;

        private event Action _onTick;
        public event Action OnTick
        {
            add
            {
                _onTick += value;
                _subscriberCount++;
                if (!enabled)
                {
                    enabled = true;
                }
            }
            remove
            {
                _onTick -= value;
                _subscriberCount--;
                if (enabled && _subscriberCount == 0)
                {
                    enabled = false;
                }
            }
        }

        protected void Tick()
        {
            _onTick?.Invoke();
        }
    }
}