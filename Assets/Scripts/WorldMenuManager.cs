using Oculus.Interaction.Locomotion;
using UnityEngine;

public class WorldMenuManager : MonoBehaviour
{
    private ObjectScentManager ObjectScentManager;
    private GameObject MenuCanvas;
    private GameObject SearchingObject;
    private FirstPersonLocomotor PlayerController;
    private float SpeedFactor;
    private float CrouchSpeedFactor;
    private float RunningSpeedFactor;

    private void Awake()
    {
        ObjectScentManager = GetComponent<ObjectScentManager>();
        MenuCanvas = GameObject.Find("MenuCanvas");
        SearchingObject = GameObject.Find("SearchingObject");
        PlayerController = GameObject.Find("PlayerController").GetComponent<FirstPersonLocomotor>();
        SpeedFactor = PlayerController.SpeedFactor;
        CrouchSpeedFactor = PlayerController.CrouchSpeedFactor;
        RunningSpeedFactor = PlayerController.RunningSpeedFactor;
}

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            if(!ObjectScentManager.MenuOn)
            {
                ObjectScentManager.MenuOn = true;
                PlayerController.SpeedFactor = 0;
                PlayerController.CrouchSpeedFactor = 0;
                PlayerController.RunningSpeedFactor = 0;
                MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 1.75f);
                SearchingObject.GetComponent<CanvasGroup>().alpha = 0;
                
            } else if(ObjectScentManager.MenuOn)
            {
                ObjectScentManager.MenuOn = false;
                PlayerController.SpeedFactor = 30;
                PlayerController.CrouchSpeedFactor = 10;
                PlayerController.RunningSpeedFactor = 50;
                SearchingObject.GetComponent<CanvasGroup>().alpha = 1;
                MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, -2f);
            }
        }
    }

    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void GoBack()
    {
        ObjectScentManager.MenuOn = false;
        SearchingObject.GetComponent<CanvasGroup>().alpha = 1;
        MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, -2f);
    }
}
