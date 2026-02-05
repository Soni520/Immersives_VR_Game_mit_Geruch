using System.Security.Cryptography;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    private OlfactoryDeviceManager OlfactoryDeviceManager;
    public TextMeshProUGUI minute_text;
    public TextMeshProUGUI scent_text;

    private int currentMinutes = 5;
    private int currentScentPercent = 50;

    private const int MIN_MINUTES = 1;
    private const int MAX_MINUTES = 10;
    private const int MIN_SCENT = 0;
    private const int MAX_SCENT = 100;
    private const int SCENT_STEP = 10;

    private bool OlfactoryStarted = false;
    private bool TestFrequencyRunning = false;
    private float TestFrequencyTimer = 0f;

private void Awake()
    {
        OlfactoryDeviceManager = GetComponent<OlfactoryDeviceManager>();
    }
    void Start()
    {

        // Gespeicherte Werte laden
        int savedTimeValue = PlayerPrefs.GetInt("TimeValue", 1);
        currentMinutes = savedTimeValue;

        int savedScentValue = PlayerPrefs.GetInt("ScentIntensity", 50);
        currentScentPercent = savedScentValue;

        UpdateMinuteText();
        UpdateScentText();
    }

    void Update()
    {
        if (TestFrequencyRunning)
        {
            TestFrequencyTimer += Time.deltaTime;
            if (TestFrequencyTimer > 5f)
            {
                OlfactoryDeviceManager.StopAllPumps();
                TestFrequencyRunning = false;
                TestFrequencyTimer = 0f;
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
        if (currentScentPercent >= MIN_SCENT + SCENT_STEP)
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
        
        if (currentScentPercent <= MAX_SCENT - SCENT_STEP)
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
        PlayerPrefs.SetInt("TimeValue", currentMinutes);
        PlayerPrefs.Save();
        Debug.Log("Zeit gespeichert: " + currentMinutes + " Minuten");
    }

    private void SaveScentValue()
    {
        PlayerPrefs.SetInt("ScentIntensity", currentScentPercent);
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
        OlfactoryDeviceManager.StopAllPumps();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public static int GetTimeValue()
    {
        return PlayerPrefs.GetInt("TimeValue", 5);
    }

    public static int GetScentIntensity()
    {
        return PlayerPrefs.GetInt("ScentIntensity", 50);
    }

    public void TestFrequency()
    {
        OlfactoryDeviceManager.SetPump(1);
        OlfactoryDeviceManager.SetFrequency((double)(PlayerPrefs.GetInt("ScentIntensity", 50) * 1.5));
        OlfactoryDeviceManager.StartPump();
        TestFrequencyRunning = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
