using UnityEngine;
using TMPro;
using UnityEditor;

public class BreathingFade : MonoBehaviour
{
    private GameObject MenuCanvas;
    public Renderer sphereRenderer;

    public float minAlpha = 0.05f;
    public float maxAlpha = 0.6f;
    public float speed = 0.2f;

    public TextMeshProUGUI breathText;

    [Header("Breathe in")]
    public float inhaleLogStrength = 4f;
    public float inhaleScale = 1.08f;
    public Color inhaleColor = new Color(0.7f, 0.85f, 1f);

    float lastA;
    Vector3 baseScale;

    void Start()
    {
        baseScale = breathText.transform.localScale;
        MenuCanvas = GameObject.Find("MenuCanvas");
    }

    void Update()
    {
        if (MenuCanvas.transform.position.z >= 0f)
        {
            return;
        }else if (MenuCanvas.transform.position.z < 0f){
            float t = Mathf.PingPong(Time.time * speed, 1f);

            // Sphere
            float a = Mathf.Lerp(minAlpha, maxAlpha, t);
            Color sphereColor = sphereRenderer.material.color;
            sphereColor.a = a;
            sphereRenderer.material.color = sphereColor;

            Color textColor = breathText.color;

            if (a < lastA)
            {
                // Breath In (heller werdend)
                breathText.text = "Breathe in";

                // Fortschritt korrekt herum
                float p = 1f - t;

                // logarithmisch EINblenden
                float logP = 1f - Mathf.Exp(-inhaleLogStrength * p);
                logP = Mathf.Clamp01(logP);

                // Alpha
                textColor.a = logP;

                // Farbe Richtung blau
                textColor.r = Mathf.Lerp(1f, inhaleColor.r, logP);
                textColor.g = Mathf.Lerp(1f, inhaleColor.g, logP);
                textColor.b = Mathf.Lerp(1f, inhaleColor.b, logP);

                // Skalierung: gr��er werden
                float scale = Mathf.Lerp(1f, inhaleScale, logP);
                breathText.transform.localScale = baseScale * scale;
            }
            else
            {
                // Breath Out (dunkler werdend)
                breathText.text = "Breathe out";

                // linear AUSblenden
                float outP = t;

                textColor.a = 1f - outP;

                // Farbe zur�ck zu wei�
                textColor.r = 1f;
                textColor.g = 1f;
                textColor.b = 1f;

                // Skalierung: kleiner werden
                float scale = Mathf.Lerp(inhaleScale, 1f, outP);
                breathText.transform.localScale = baseScale * scale;
            }

            breathText.color = textColor;
            lastA = a;
        }
    }
}
