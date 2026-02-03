using System.Security.Cryptography;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    private ObjectScentManager ObjectScentManager;
    private OlfactoryDeviceManager OlfactoryDeviceManager;

    private void Awake()
    {
        ObjectScentManager = GetComponent<ObjectScentManager>();
        OlfactoryDeviceManager = GetComponent<OlfactoryDeviceManager>();
    }
    public TextMeshProUGUI minute_text;
    public TextMeshProUGUI scent_text;

    private int currentMinutes = 5;
    private int currentScentPercent = 100;

    private const int MIN_MINUTES = 1;
    private const int MAX_MINUTES = 10;
    private const int MIN_SCENT = 0;
    private const int MAX_SCENT = 100;
    private const int SCENT_STEP = 10;

    private bool OlfactoryStarted = false;

    void Start()
    {

        // Gespeicherte Werte laden
        float savedTimeValue = PlayerPrefs.GetFloat("TimeValue", 1.0f);
        currentMinutes = Mathf.RoundToInt(Mathf.Lerp(MIN_MINUTES, MAX_MINUTES, savedTimeValue));

        float savedScentValue = PlayerPrefs.GetFloat("ScentIntensity", 1.0f);
        currentScentPercent = Mathf.RoundToInt(savedScentValue * 100f);

        UpdateMinuteText();
        UpdateScentText();
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().interactable = !GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().interactable;
            GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().blocksRaycasts = !GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().blocksRaycasts;
            if(GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().alpha == 0)
            {
                GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().alpha = 1;
                ObjectScentManager.MenuOn = true;
                GameObject.Find("SearchingObject").GetComponent<CanvasGroup>().alpha = 0;
                
            } else if(GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().alpha == 1)
            {
                GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
                ObjectScentManager.MenuOn = false;
                GameObject.Find("SearchingObject").GetComponent<CanvasGroup>().alpha = 1;
                
            }
        }
    }

    // Zeit-Buttons
    public void OnTimeMinusClicked()
    {
        if (currentMinutes > MIN_MINUTES)
        {
            currentMinutes--;
            SaveTimeValue();
            UpdateMinuteText();
        }
    }

    public void OnTimePlusClicked()
    {
        if (currentMinutes < MAX_MINUTES)
        {
            currentMinutes++;
            SaveTimeValue();
            UpdateMinuteText();
        }
    }

    // Geruchs-Buttons
    public void OnScentMinusClicked()
    {
        if (currentScentPercent > MIN_SCENT)
        {
            currentScentPercent -= SCENT_STEP;
            SaveScentValue();
            UpdateScentText();
        }

        if (!OlfactoryStarted)
        {
            OlfactoryDeviceManager.Open();
            OlfactoryDeviceManager.Open();
            OlfactoryStarted = true;
        }
    }

    public void OnScentPlusClicked()
    {
        if (currentScentPercent < MAX_SCENT)
        {
            currentScentPercent += SCENT_STEP;
            SaveScentValue();
            UpdateScentText();
        }

        if (!OlfactoryStarted)
        {
            OlfactoryDeviceManager.Open();
            OlfactoryDeviceManager.Open();
            OlfactoryStarted = true;
        }
    }

    private void SaveTimeValue()
    {
        float normalizedValue = Mathf.InverseLerp(MIN_MINUTES, MAX_MINUTES, currentMinutes);
        PlayerPrefs.SetFloat("TimeValue", normalizedValue);
        PlayerPrefs.Save();
        Debug.Log("Zeit gespeichert: " + currentMinutes + " Minuten");
    }

    private void SaveScentValue()
    {
        float normalizedValue = currentScentPercent / 100f;
        PlayerPrefs.SetFloat("ScentIntensity", normalizedValue);
        PlayerPrefs.Save();
        Debug.Log("Geruchsintensität gespeichert: " + currentScentPercent + "%");
    }

    void UpdateMinuteText()
    {
        if (minute_text != null)
        {
            minute_text.text = currentMinutes + " Minute" + (currentMinutes > 1 ? "n" : "");
        }
    }

    void UpdateScentText()
    {
        if (scent_text != null)
        {
            scent_text.text = currentScentPercent + "%";
        }
    }

    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void GoBack()
    {
        GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().blocksRaycasts = false;
        ObjectScentManager.MenuOn = false;
        GameObject.Find("SearchingObject").GetComponent<CanvasGroup>().alpha = 1;
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
