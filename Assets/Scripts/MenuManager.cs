using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Slider settingsSlider; // Slider-Referenz im Inspector zuweisen

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Gespeicherten Wert laden (Standard: 1.0)
        if (settingsSlider != null)
        {
            settingsSlider.value = PlayerPrefs.GetFloat("SliderValue", 1.0f);
            settingsSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            GameObject.Find("MenuCanvas").GetComponent<Canvas>().enabled = !GameObject.Find("MenuCanvas").GetComponent<Canvas>().enabled;
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

    void OnSliderValueChanged(float value)
    {
        // Wert in PlayerPrefs speichern
        PlayerPrefs.SetFloat("SliderValue", value);
        PlayerPrefs.Save();
        Debug.Log("Slider Wert gespeichert: " + value);
    }

    // Statische Methode zum Abrufen des Werts aus anderen Szenen
    public static float GetSliderValue()
    {
        return PlayerPrefs.GetFloat("SliderValue", 1.0f);
    }
}
