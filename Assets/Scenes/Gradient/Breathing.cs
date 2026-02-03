using UnityEngine;

public class BreathingFade : MonoBehaviour
{
    public Renderer sphereRenderer;

    public float minAlpha = 0.05f;
    public float maxAlpha = 0.6f;
    public float speed = 0.2f;

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);
        float a = Mathf.Lerp(minAlpha, maxAlpha, t);

        Color c = sphereRenderer.material.color;
        c.a = a;
        sphereRenderer.material.color = c;
    }
}
