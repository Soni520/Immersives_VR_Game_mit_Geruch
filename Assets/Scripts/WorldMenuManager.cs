using Oculus.Interaction.Locomotion;
using UnityEditor;
using UnityEngine;

/*
 * This script manages the UI menu in the world.
 */
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

        // Variables to store the original movement speeds
        SpeedFactor = PlayerController.SpeedFactor;
        CrouchSpeedFactor = PlayerController.CrouchSpeedFactor;
        RunningSpeedFactor = PlayerController.RunningSpeedFactor;
}

    void Update()
    {
        // Check if the Menu button is pressed
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            // If menu is closed, open it
            if (!ObjectScentManager.MenuOn)
            {
                ObjectScentManager.MenuOn = true;

                // Freeze player movement
                PlayerController.SpeedFactor = 0;
                PlayerController.CrouchSpeedFactor = 0;
                PlayerController.RunningSpeedFactor = 0;

                // Move the menu forward to make it visible
                MenuPosition.z = 2f;
                MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = MenuPosition;

                // Hide the searching text
                SearchingObject.GetComponent<CanvasGroup>().alpha = 0;
                
            } else if(ObjectScentManager.MenuOn)   // If menu is open
            {
                ObjectScentManager.MenuOn = false;

                // Reset original movement speeds
                PlayerController.SpeedFactor = SpeedFactor;
                PlayerController.CrouchSpeedFactor = CrouchSpeedFactor;
                PlayerController.RunningSpeedFactor = RunningSpeedFactor;

                // Show the searching text
                SearchingObject.GetComponent<CanvasGroup>().alpha = 1;

                // Hide the menu
                MenuPosition.z = -2f;
                MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = MenuPosition;
            }
        }
    }

    // Handle scene transition
    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    // Close menu und continue with the game 
    public void GoBack()
    {
        // Reset original movement speeds
        PlayerController.SpeedFactor = SpeedFactor;
        PlayerController.CrouchSpeedFactor = CrouchSpeedFactor;
        PlayerController.RunningSpeedFactor = RunningSpeedFactor;

        ObjectScentManager.MenuOn = false;

        // Show the searching text
        SearchingObject.GetComponent<CanvasGroup>().alpha = 1;
        
        // Hide the menu
        MenuPosition.z = -2f;
        MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = MenuPosition;
    }
}
