using UnityEngine;

public class GradientMenuManager : MonoBehaviour
{
    private OlfactoryDeviceManager OlfactoryDeviceManager;
    private GameObject MenuCanvas;
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

        if (!Initialized)
        {
            InitializeTimer += Time.deltaTime;
            if (InitializeTimer > 3.0f)
            {
                Initialized = true;
                OlfactoryDeviceManager.Open();
                OlfactoryDeviceManager.Open();
                OlfactoryDeviceManager.SetPump(Random.Range(1, 4));
                OlfactoryDeviceManager.SetFrequency(PlayerPrefs.GetFloat("ScentIntensity", 1.0f) * 1.5f);
                OlfactoryDeviceManager.StartPump();
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
    }

    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void GoBack()
    {
        MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, -2f);
        MenuOn = false;
    }
}
