using UnityEngine;

public class BreathingFade : MonoBehaviour
{
    public Renderer sphereRenderer;

    public float minAlpha = 0.05f;
    public float maxAlpha = 0.6f;
    public float breathingSpeed = 0.2f;

    void Update()
    {
        float t = Mathf.PingPong(Time.time * breathingSpeed, 1f);
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, t);

        Color c = sphereRenderer.material.color;
        c.a = alpha;
        sphereRenderer.material.color = c;
    }
}
