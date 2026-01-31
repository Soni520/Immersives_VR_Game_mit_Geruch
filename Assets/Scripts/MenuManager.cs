using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public Slider timeSlider;
    public Slider scentSlider;
    public TextMeshProUGUI minute_text; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Gespeicherte Werte laden (Standard: 1.0)
        if (timeSlider != null)
        {
            timeSlider.value = PlayerPrefs.GetFloat("TimeValue", 1.0f);
            timeSlider.onValueChanged.AddListener(OnTimeSliderValueChanged);
            UpdateMinuteText(timeSlider.value);
        }

        if (scentSlider != null)
        {
            scentSlider.value = PlayerPrefs.GetFloat("ScentIntensity", 1.0f);
            scentSlider.onValueChanged.AddListener(OnScentSliderValueChanged);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            GameObject.Find("MenuCanvas").GetComponent<Canvas>().enabled = !GameObject.Find("MenuCanvas").GetComponent<Canvas>().enabled;
        }

        // Slider-Steuerung mit Thumbsticks
        HandleSliderInput();
    }

    void HandleSliderInput()
    {
        // Linker Thumbstick (Y-Achse) steuert timeSlider
        if (timeSlider != null)
        {
            Vector2 leftThumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            if (Mathf.Abs(leftThumbstick.y) > 0.1f) // Deadzone von 0.1
            {
                float newValue = timeSlider.value + (leftThumbstick.y * Time.deltaTime * 0.5f);
                timeSlider.value = Mathf.Clamp01(newValue);
            }
        }

        // Rechter Thumbstick (Y-Achse) steuert scentSlider
        if (scentSlider != null)
        {
            Vector2 rightThumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
            if (Mathf.Abs(rightThumbstick.y) > 0.1f) // Deadzone von 0.1
            {
                float newValue = scentSlider.value + (rightThumbstick.y * Time.deltaTime * 0.5f);
                scentSlider.value = Mathf.Clamp01(newValue);
            }
        }
    }
 
    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void GoBack()
    {
        GameObject.Find("MenuCanvas").GetComponent<Canvas>().enabled = false;
    }

    void OnTimeSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat("TimeValue", value);
        PlayerPrefs.Save();
        Debug.Log("Time Slider Wert gespeichert: " + value);
        UpdateMinuteText(value);
    }

    void UpdateMinuteText(float sliderValue)
    {
        if (minute_text != null)
        {
            // Slider-Wert von 0-1 auf 1-10 Minuten mappen
            int minutes = Mathf.RoundToInt(Mathf.Lerp(1f, 10f, sliderValue));
            minute_text.text = minutes + " Minute" + (minutes > 1 ? "n" : "");
        }
    }

    void OnScentSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat("ScentIntensity", value);
        PlayerPrefs.Save();
        Debug.Log("Scent Slider Wert gespeichert: " + value);
    }

    public static float GetTimeValue()
    {
        return PlayerPrefs.GetFloat("TimeValue", 1.0f);
    }

    public static float GetScentIntensity()
    {
        return PlayerPrefs.GetFloat("ScentIntensity", 1.0f);
    }
}
