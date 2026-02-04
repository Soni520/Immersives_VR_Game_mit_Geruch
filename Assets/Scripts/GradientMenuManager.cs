using UnityEngine;

public class GradientMenuManager : MonoBehaviour
{
    private OlfactoryDeviceManager OlfactoryDeviceManager;
    private bool Initialized = false;
    private float InitializeTimer = 0f;
    void Awake()
    {
        OlfactoryDeviceManager = GetComponent<OlfactoryDeviceManager>();
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
                OlfactoryDeviceManager.SetFrequency(0);

                
            } else if(GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().alpha == 1)
            {
                GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
                OlfactoryDeviceManager.SetFrequency(PlayerPrefs.GetFloat("ScentIntensity", 1.0f) * 1.5f);
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
        OlfactoryDeviceManager.SetFrequency(PlayerPrefs.GetFloat("ScentIntensity", 1.0f) * 1.5f);
    }
}
