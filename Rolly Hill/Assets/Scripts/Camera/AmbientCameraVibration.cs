using UnityEngine;

public class AmbientCameraVibration : MonoBehaviour
{
    public float amplitude = 0.03f;
    public float speed = 1f;

    Vector3 startPos;
    float seed;

    void Start()
    {
        startPos = transform.localPosition;
        seed = Random.value * 100f;
    }

    void Update()
    {
        float x = (Mathf.PerlinNoise(seed, Time.time * speed) - 0.5f) * amplitude;
        float y = (Mathf.PerlinNoise(seed + 1f, Time.time * speed) - 0.5f) * amplitude;

        transform.localPosition = startPos + new Vector3(x, y, 0);
    }
}

