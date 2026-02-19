using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GradientMenuManager : MonoBehaviour
{
    private OlfactoryDeviceManager OlfactoryDeviceManager;
    private GameObject MenuCanvas;
    private GameObject BreathingCanvas;
    private GameObject VideoPlayer;
    private GameObject MusicPlayer;
    private float Timer = 0f;
    private bool MenuOn = false;
    private Vector3 MenuPosition;
    private Vector3 BreathingPosition;
    void Awake()
    {
        MenuCanvas = GameObject.Find("MenuCanvas");
        BreathingCanvas = GameObject.Find("BreathingCanvas");
        VideoPlayer = GameObject.Find("Video Player");
        MusicPlayer = GameObject.Find("Music Player");
        OlfactoryDeviceManager = GetComponent<OlfactoryDeviceManager>();
        MenuPosition = MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D;
        BreathingPosition = BreathingCanvas.GetComponent<RectTransform>().anchoredPosition3D;
    }

    void Start()
    {
        OlfactoryDeviceManager.SetPump(PlayerPrefs.GetInt("CurrentFruit", Random.Range(1, 4)));
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
                MenuPosition.z = 1.75f;
                BreathingPosition.z = -2f;
                MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = MenuPosition;
                BreathingCanvas.GetComponent<RectTransform>().anchoredPosition3D = BreathingPosition;
                VideoPlayer.GetComponent<UnityEngine.Video.VideoPlayer>().Pause();
                MusicPlayer.GetComponent<AudioSource>().Pause();
                OlfactoryDeviceManager.StopAllPumps();
            } else if(MenuOn)
            {
                MenuOn = false;
                MenuPosition.z = -2f;
                BreathingPosition.z = 1.75f;
                MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = MenuPosition;
                BreathingCanvas.GetComponent<RectTransform>().anchoredPosition3D = BreathingPosition;
                VideoPlayer.GetComponent<UnityEngine.Video.VideoPlayer>().Play();
                MusicPlayer.GetComponent<AudioSource>().Play();
                OlfactoryDeviceManager.StartPump();
            }
        }

        if (!MenuOn)
        {
            Timer += Time.deltaTime;   
        }

        if (Timer > PlayerPrefs.GetInt("TimeValue", 1) * 60f)
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
        MenuPosition.z = -2f;
        BreathingPosition.z = 1.75f;
        MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = MenuPosition;
        BreathingCanvas.GetComponent<RectTransform>().anchoredPosition3D = BreathingPosition;
        VideoPlayer.GetComponent<UnityEngine.Video.VideoPlayer>().Play();
        MusicPlayer.GetComponent<AudioSource>().Play();
        OlfactoryDeviceManager.StartPump();
        MenuOn = false;
    }

    public void SetModi(int modi)
    {
        PlayerPrefs.SetInt("MeditationModi", modi);
    }
}
