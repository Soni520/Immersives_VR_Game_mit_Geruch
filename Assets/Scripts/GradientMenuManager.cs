using UnityEngine;

public class GradientMenuManager : MonoBehaviour
{
    private OlfactoryDeviceManager OlfactoryDeviceManager;
    private GameObject MenuCanvas;
    private float Timer = 0f;
    private bool MenuOn = false;
    void Awake()
    {
        MenuCanvas = GameObject.Find("MenuCanvas");
        OlfactoryDeviceManager = GetComponent<OlfactoryDeviceManager>();
    }

    void Start()
    {
        OlfactoryDeviceManager.SetPump(Random.Range(1, 4));
        OlfactoryDeviceManager.SetFrequency(PlayerPrefs.GetInt("ScentIntensity", 50) * 1.5f);
        OlfactoryDeviceManager.StartPump();
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
