using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/*
 * This script manages the UI menu in the meditation scene
 */
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

        // Store initial 3D positions of the UI elements
        MenuPosition = MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D;
        BreathingPosition = BreathingCanvas.GetComponent<RectTransform>().anchoredPosition3D;
    }

    void Start()
    {
        // Initialize the scent device based on current fruit and choosen intensity
        OlfactoryDeviceManager.SetPump(PlayerPrefs.GetInt("CurrentFruit", Random.Range(1, 4)));
        OlfactoryDeviceManager.SetFrequency(PlayerPrefs.GetInt("ScentIntensity", 50) * 1.5f);
        OlfactoryDeviceManager.StartPump();
        PlayerPrefs.SetInt("Phase", 0); // Phase 0 - begin of the meditation
    }

    void Update()
    {
        // Check if the Menu button is pressed
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            // If menu is closed, open it
            if (!MenuOn)
            {
                MenuOn = true;

                // Move the menu forward to make it visible and hide the breathing text
                MenuPosition.z = 2f;
                BreathingPosition.z = -2f;
                MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = MenuPosition;
                BreathingCanvas.GetComponent<RectTransform>().anchoredPosition3D = BreathingPosition;

                // Pause media
                VideoPlayer.GetComponent<UnityEngine.Video.VideoPlayer>().Pause();
                MusicPlayer.GetComponent<AudioSource>().Pause();

                // Stop scent release
                OlfactoryDeviceManager.StopAllPumps();
            } else if(MenuOn)   // If menu is open
            {
                MenuOn = false;

                // Hide the menu and show breathing text
                MenuPosition.z = -2f;
                BreathingPosition.z = 2f;
                MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = MenuPosition;
                BreathingCanvas.GetComponent<RectTransform>().anchoredPosition3D = BreathingPosition;
                
                // Resuming media
                VideoPlayer.GetComponent<UnityEngine.Video.VideoPlayer>().Play();
                MusicPlayer.GetComponent<AudioSource>().Play();

                // Reactivate scent
                OlfactoryDeviceManager.StartPump();
            }
        }

        // Only progress the session timer if the menu is not active
        if (!MenuOn)
        {
            Timer += Time.deltaTime;   
        }

        // Check if the session duration has been reached
        if (Timer > PlayerPrefs.GetInt("TimeValue", 1) * 60f)
        {
            ChangeScene("Menu");
        }
    }

    // Handle scene transition
    public void ChangeScene(string sceneName)
    {
        // Stops all pumps before changes scene
        OlfactoryDeviceManager.StopAllPumps();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    // Close menu und continue with the game 
    public void GoBack()
    {
        MenuOn = false;

        // Hide the menu and show the breathing text
        MenuPosition.z = -2f;
        BreathingPosition.z = 2f;
        MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = MenuPosition;
        BreathingCanvas.GetComponent<RectTransform>().anchoredPosition3D = BreathingPosition;

        // Resuming media
        VideoPlayer.GetComponent<UnityEngine.Video.VideoPlayer>().Play();
        MusicPlayer.GetComponent<AudioSource>().Play();

        // Reactivate scent
        OlfactoryDeviceManager.StartPump();
    }
}
