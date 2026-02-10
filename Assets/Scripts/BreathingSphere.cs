using UnityEngine;
using TMPro;

public class BreathingFade : MonoBehaviour
{
    public Renderer sphereRenderer;

    public float minAlpha = 0.05f;
    public float maxAlpha = 0.6f;
    public float speed = 0.2f;

    public TextMeshProUGUI breathText;

    public float inhaleLogStrength = 4f; // Stärke der Log-Kurve

    float lastA;

    void Update()
    {
        // 0..1 Atemzyklus
        float t = Mathf.PingPong(Time.time * speed, 1f);

        // Sphere Alpha
        float a = Mathf.Lerp(minAlpha, maxAlpha, t);
        Color sphereColor = sphereRenderer.material.color;
        sphereColor.a = a;
        sphereRenderer.material.color = sphereColor;

        // Text-Logik
        Color textColor = breathText.color;

        if (a < lastA)
        {
            // Breath In (heller werdend)
            breathText.text = "Breath In";

            // logarithmisch: schnell sichtbar, dann langsam
            float logAlpha = 1f - Mathf.Exp(-inhaleLogStrength * t);
            textColor.a = Mathf.Clamp01(logAlpha);
        }
        else
        {
            // Breath Out (dunkler werdend)
            breathText.text = "Breath Out";

            // linear fade out
            textColor.a = 1f - t;
        }

        breathText.color = textColor;
        lastA = a;
    }
}
