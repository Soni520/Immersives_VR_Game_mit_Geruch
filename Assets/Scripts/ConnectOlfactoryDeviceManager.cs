using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/*
 * This script ensures a stable connection to the Olfactory device by 
 * attempting to open the serial port after a brief initialization delay.
 */
public class ConnectOlfactoryDeviceManager : MonoBehaviour
{
    private OlfactoryDeviceManager OlfactoryDeviceManager;
    void Awake()
    {
        OlfactoryDeviceManager = GetComponent<OlfactoryDeviceManager>();

        // Start the delayed connection sequence
        StartCoroutine(OpenWithDelay());
    }

    /*
     * Coroutine that waits for the initial frames to pass before 
     * attempting to open the serial port connection
     */
    IEnumerator OpenWithDelay()
    {
        // Loop twice to ensure we wait at least two frames
        for (int i = 0; i < 2; i++)
        {
            // Wait until the next frame
            yield return null;

            try
            {
                // Attempt to open the serial port connection on the Meta Quest
                OlfactoryDeviceManager.Open();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}