using UnityEngine;
using TMPro;

public class GradientMenuManager : MonoBehaviour
{
    private OlfactoryDeviceManager OlfactoryDeviceManager;
    private GameObject MenuCanvas;
    [SerializeField] private TextMeshProUGUI OlfactoryDebugText;
    private bool Initialized = false;
    private float InitializeTimer = 0f;
    private float Timer = 0f;
    private bool MenuOn = false;
    void Awake()
    {
        OlfactoryDeviceManager = GetComponent<OlfactoryDeviceManager>();
        MenuCanvas = GameObject.Find("MenuCanvas");
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            if(!MenuOn)
            {
                MenuOn = true;
                MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 1.75f);
                
            } else if(MenuOn)
            {
                MenuOn = false;
                MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, -2f);
            }
        }

        if (!MenuOn)
        {
            Timer += Time.deltaTime;   
        }

        if (Timer > PlayerPrefs.GetInt("TimeValue", 5) * 60f)
        {
            ChangeScene("Menu");
        }

        if (!Initialized && Timer > 3f)
        {
            //OlfactoryDebugText.text = "Timer: " + Timer.ToString("F2") + " | vor Open 1";
            OlfactoryDebugText.text = OlfactoryDeviceManager.ToString();
            OlfactoryDeviceManager.Open();
            OlfactoryDebugText.text = "Timer: " + Timer.ToString("F2") + " | nach Open 1";
            OlfactoryDeviceManager.Open();
            OlfactoryDebugText.text = "Timer: " + Timer.ToString("F2") + " | nach Open 2";
            OlfactoryDeviceManager.SetPump(Random.Range(1, 4));
            OlfactoryDebugText.text = "Timer: " + Timer.ToString("F2") + " | nach setPump";
            OlfactoryDeviceManager.SetFrequency(PlayerPrefs.GetFloat("ScentIntensity", 1.0f) * 1.5f);
            OlfactoryDebugText.text = "Timer: " + Timer.ToString("F2") + " | nach SetFrequency";
            OlfactoryDeviceManager.StartPump();
            OlfactoryDebugText.text = "Timer: " + Timer.ToString("F2") + " | nach StartPump";
            Initialized = true;
        }

    }

    public void ChangeScene(string sceneName)
    {
        OlfactoryDeviceManager.StopAllPumps();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void GoBack()
    {
        MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, -2f);
        MenuOn = false;
    }
}
