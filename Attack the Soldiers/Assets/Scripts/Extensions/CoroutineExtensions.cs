using UnityEngine;

public static class CoroutineExtensions
{
    public static void StopCoroutineSafe(this MonoBehaviour monoBehaviour, ref Coroutine coroutine)
    {
        if (coroutine != null)
        {
            monoBehaviour.StopCoroutine(coroutine);
            coroutine = null;
        }
    }
}
