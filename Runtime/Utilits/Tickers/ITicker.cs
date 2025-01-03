using System;

public interface ITicker
{
    public event Action OnTick;
}