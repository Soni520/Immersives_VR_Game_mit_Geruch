using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public class BreathingFade : MonoBehaviour
{
    [SerializeField] private GameObject Modi;
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
    private int Phase = 0;
    private float Timer = -4;
    private float t = 0;

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
            PlaySelectedModi();
            if (Phase != 1 && Phase != 3){
                t = Mathf.PingPong(Timer * speed, 1f);
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

    private void PlaySelectedModi()
    {
        Timer += Time.deltaTime;
        Phase = PlayerPrefs.GetInt("Phase", 0);
        switch (PlayerPrefs.GetInt("CurrentFruit", 1))
        {
            // Normal
            case 1:
                speed = ((maxAlpha - minAlpha) / 3);
                break;
            // Vier Sekunden einatmen, vier Sekunden halten, vier Sekunden ausatmen, vier Sekunden halten.
            case 2:
                switch (Phase)
                {
                    case 0:
                        if(Timer <= 0)
                        {
                            speed = ((maxAlpha - minAlpha) / 4);
                            break;
                        }
                        else
                        {
                            PlayerPrefs.SetInt("Phase", 1);
                            Timer = 0;
                            break;
                        }
                    case 1:
                        breathText.text = "Hold your breath";
                        if(Timer > 4)
                        {
                            PlayerPrefs.SetInt("Phase", 2);
                            Timer = 0;
                            break;
                        }
                        break;
                    case 2:
                        if(Timer <= 4)
                        {
                            speed = ((maxAlpha - minAlpha) / 4);
                            break;
                        }
                        else
                        {
                            PlayerPrefs.SetInt("Phase", 3);
                            Timer = 0;
                            break;
                        }
                    case 3:
                        breathText.text = "Hold your breath";

                        // linear EINblenden
                        float inP = 1 - t/2;

                        Color textColor = breathText.color;
                        textColor.a = inP;

                        // Farbe Richtung blau
                        textColor.r = Mathf.Lerp(1f, inhaleColor.r, inP);
                        textColor.g = Mathf.Lerp(1f, inhaleColor.g, inP);
                        textColor.b = Mathf.Lerp(1f, inhaleColor.b, inP);

                        // Skalierung: gr��er werden
                        float scale = Mathf.Lerp(1f, inhaleScale, inP);
                        breathText.transform.localScale = baseScale * scale;

                        // linear AUSblenden
                        float outP = t/2;

                        textColor = breathText.color;
                        textColor.a = 1f - outP;

                        // Farbe zur�ck zu wei�
                        textColor.r = 1f;
                        textColor.g = 1f;
                        textColor.b = 1f;

                        // Skalierung: kleiner werden
                        scale = Mathf.Lerp(inhaleScale, 1f, outP);
                        breathText.transform.localScale = baseScale * scale;

                        if(Timer > 4)
                        {
                            PlayerPrefs.SetInt("Phase", 0);
                            Timer = -4;
                            break;
                        }
                        break;
                }
                break;
            // 4 Sekunden durch die Nase einatmen, 7 Sekunden Atem anhalten, 8 Sekunden ausatmen
            case 3:
                switch (Phase)
                {
                    case 0:
                        if(Timer <= 0)
                        {
                            speed = ((maxAlpha - minAlpha) / 4);
                            break;
                        }
                        else
                        {
                            PlayerPrefs.SetInt("Phase", 1);
                            Timer = 0;
                            break;
                        }
                    case 1:
                        breathText.text = "Hold your breath";
                        if(Timer > 7)
                        {
                            PlayerPrefs.SetInt("Phase", 2);
                            Timer = 0;
                        }
                        break;
                    case 2:
                        if(Timer <= 8)
                        {
                            speed = ((maxAlpha - minAlpha) / 8);
                            break;
                        }
                        else
                        {
                            PlayerPrefs.SetInt("Phase", 0);
                            Timer = -4;
                            break;
                        }
                }
                break;
            case 4:
                switch (Phase)
                {
                    case 0:
                        if (Timer <= 0)
                        {
                            speed = ((maxAlpha - minAlpha) / 4);
                            break;
                        }
                        else
                        {
                            PlayerPrefs.SetInt("Phase", 1);
                            Timer = 0;
                            break;
                        }
                    case 1:
                        PlayerPrefs.SetInt("Phase", 2);
                        Timer = 0;
                        break;
                    case 2:
                        if (Timer <= 6)
                        {
                            speed = ((maxAlpha - minAlpha) / 6);
                            break;
                        }
                        else
                        {
                            PlayerPrefs.SetInt("Phase", 0);
                            Timer = -4;
                            break;
                        }
        
                }
                break;
        }
    }
}
