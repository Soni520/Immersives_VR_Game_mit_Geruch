using Oculus.Interaction.Locomotion;
using UnityEditor;
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
    private Vector3 MenuPosition;

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
                MenuPosition.z = 1.75f;
                MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = MenuPosition;
                SearchingObject.GetComponent<CanvasGroup>().alpha = 0;
                
            } else if(ObjectScentManager.MenuOn)
            {
                ObjectScentManager.MenuOn = false;
                MenuPosition.z = -2f;
                PlayerController.SpeedFactor = SpeedFactor;
                PlayerController.CrouchSpeedFactor = CrouchSpeedFactor;
                PlayerController.RunningSpeedFactor = RunningSpeedFactor;
                SearchingObject.GetComponent<CanvasGroup>().alpha = 1;
                MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = MenuPosition;
            }
        }
    }

    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void GoBack()
    {
        PlayerController.SpeedFactor = SpeedFactor;
        PlayerController.CrouchSpeedFactor = CrouchSpeedFactor;
        PlayerController.RunningSpeedFactor = RunningSpeedFactor;
        ObjectScentManager.MenuOn = false;
        MenuPosition.z = -2f;
        SearchingObject.GetComponent<CanvasGroup>().alpha = 1;
        MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = MenuPosition;
    }
}
