using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

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
        // Raycast von beiden Controllern
        HandleControllerRaycast(OVRInput.Controller.RTouch);
        HandleControllerRaycast(OVRInput.Controller.LTouch);
    }

    void HandleControllerRaycast(OVRInput.Controller controller)
    {
        // Nur wenn Index Trigger gedrückt wird
        bool triggerPressed = false;
        if (controller == OVRInput.Controller.RTouch)
            triggerPressed = OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
        else if (controller == OVRInput.Controller.LTouch)
            triggerPressed = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);

        if (!triggerPressed) return;

        // Controller Position und Rotation holen
        Transform controllerTransform = null;
        if (controller == OVRInput.Controller.RTouch)
        {
            GameObject rightController = GameObject.Find("RightHandAnchor");
            if (rightController == null) rightController = GameObject.Find("RightControllerAnchor");
            if (rightController != null) controllerTransform = rightController.transform;
        }
        else if (controller == OVRInput.Controller.LTouch)
        {
            GameObject leftController = GameObject.Find("LeftHandAnchor");
            if (leftController == null) leftController = GameObject.Find("LeftControllerAnchor");
            if (leftController != null) controllerTransform = leftController.transform;
        }

        if (controllerTransform == null) return;

        // Raycast für UI ausführen
        RaycastHit hit;
        if (Physics.Raycast(controllerTransform.position, controllerTransform.forward, out hit, 10f))
        {
            // Prüfen ob wir einen Slider getroffen haben
            Slider hitSlider = hit.collider.GetComponentInParent<Slider>();
            if (hitSlider != null && (hitSlider == timeSlider || hitSlider == scentSlider))
            {
                // Slider-Wert basierend auf Hit-Position berechnen
                RectTransform sliderRect = hitSlider.GetComponent<RectTransform>();
                Vector3 localHitPoint = sliderRect.InverseTransformPoint(hit.point);

                float normalizedValue = 0f;
                if (hitSlider.direction == Slider.Direction.LeftToRight || hitSlider.direction == Slider.Direction.RightToLeft)
                {
                    normalizedValue = (localHitPoint.x + sliderRect.rect.width / 2) / sliderRect.rect.width;
                    if (hitSlider.direction == Slider.Direction.RightToLeft)
                        normalizedValue = 1f - normalizedValue;
                }
                else // Vertical
                {
                    normalizedValue = (localHitPoint.y + sliderRect.rect.height / 2) / sliderRect.rect.height;
                    if (hitSlider.direction == Slider.Direction.TopToBottom)
                        normalizedValue = 1f - normalizedValue;
                }

                hitSlider.value = Mathf.Clamp01(normalizedValue);
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
