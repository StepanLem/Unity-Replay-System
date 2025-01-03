using System.Collections;
using UnityEngine;

public class CustomTicker : MonoTicker
{
    [Tooltip("Time in seconds")]
    [SerializeField] private float _interval = 1f;

    private Coroutine _tickingCoroutine;

    private void OnEnable()
    {
        _tickingCoroutine = StartCoroutine(TickCoroutine());
    }

    private void OnDisable()
    {
        StopCoroutine(_tickingCoroutine);
        _tickingCoroutine = null;
    }

    private IEnumerator TickCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_interval);
            Tick();
        }
    }
}