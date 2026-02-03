using System.Security.Cryptography;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    private OlfactoryDeviceManager OlfactoryDeviceManager;

    private void Awake()
    {
        OlfactoryDeviceManager = GetComponent<OlfactoryDeviceManager>();
    }
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

    /*void Update()
    {
        if (TestFrequencyRunning)
        {
            TestFrequencyTimer += Time.deltaTime;
            if (TestFrequencyTimer > 3f)
            {
                OlfactoryDeviceManager.StopAllPumps();
                TestFrequencyRunning = false;
                TestFrequencyTimer = 0f;
            }
        }
    }*/

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
        PlayerPrefs.SetFloat("TimeValue", currentMinutes);
        PlayerPrefs.Save();
        Debug.Log("Zeit gespeichert: " + currentMinutes + " Minuten");
    }

    private void SaveScentValue()
    {
        PlayerPrefs.SetFloat("ScentIntensity", currentScentPercent);
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

    public static float GetTimeValue()
    {
        return PlayerPrefs.GetFloat("TimeValue", 5.0f);
    }

    public static float GetScentIntensity()
    {
        return PlayerPrefs.GetFloat("ScentIntensity", 50.0f);
    }

    public void TestFrequency()
    {
        OlfactoryDeviceManager.SetPump(1);
        OlfactoryDeviceManager.SetFrequency(PlayerPrefs.GetFloat("ScentIntensity", 50.0f) * 1.5f);
        OlfactoryDeviceManager.StartPump();
        TestFrequencyRunning = true;
        System.Threading.Thread.Sleep(5000);
        OlfactoryDeviceManager.StopAllPumps();
    }
}
