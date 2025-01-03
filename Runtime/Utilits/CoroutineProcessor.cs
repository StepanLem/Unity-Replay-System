using System.Collections;
using UnityEngine;

public sealed class CoroutineProcessor : MonoBehaviour
{
    private static CoroutineProcessor _instance;

    private static CoroutineProcessor Instance
    {
        get
        {
            if (_instance == null)
            {
                var gameObject = new GameObject("[CoroutineProcessor]");
                _instance = gameObject.AddComponent<CoroutineProcessor>();
                DontDestroyOnLoad(gameObject);
            }
            return _instance;
        }
    }

    public static Coroutine StartRoutine(IEnumerator routine)
    {
        return Instance.StartCoroutine(routine);
    }

    public static void StopRoutine(Coroutine routine)
    {
        Instance.StopCoroutine(routine);
    }
}