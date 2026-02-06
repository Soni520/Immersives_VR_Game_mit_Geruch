using UnityEngine;
using System.Collections;

public class ConnectOlfactoryDeviceManager : MonoBehaviour
{
    private OlfactoryDeviceManager OlfactoryDeviceManager;
    void Awake()
    {
        OlfactoryDeviceManager = GetComponent<OlfactoryDeviceManager>();
        StartCoroutine(OpenWithDelay());
    }

    IEnumerator OpenWithDelay()
    {
        for (int i = 0; i < 2; i++)
        {
            yield return null;

            try
            {
                OlfactoryDeviceManager.Open();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}