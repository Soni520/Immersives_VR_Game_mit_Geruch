using UnityEngine;
using UnityEngine.UI;

public class BreathingFade : MonoBehaviour
{
    public Image overlayImage;

    public float minAlpha = 0.0f;   // hell
    public float maxAlpha = 0.6f;   // dunkel
    public float breathingSpeed = 0.5f; // langsamer = ruhiger

    void Update()
    {
        float t = (Mathf.Sin(Time.time * breathingSpeed * Mathf.PI * 2) + 1f) / 2f;
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, t);

        Color c = overlayImage.color;
        c.a = alpha;
        overlayImage.color = c;
    }
}