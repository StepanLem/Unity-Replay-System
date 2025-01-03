using System;

namespace StepanLem.ReplaySystem
{
    public interface ITicker
    {
        public event Action OnTick;
    }
}