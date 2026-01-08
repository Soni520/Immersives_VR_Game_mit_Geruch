using UnityEngine;

public class OlfactoryDeviceManager : MonoBehaviour
{
    protected AndroidJavaObject _androidInstanceJavaObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AndroidJavaObject androidUnityLibJavaClass = new AndroidJavaObject("com.hoho.android.usbserial.examples.TerminalFragment");
        _androidInstanceJavaObject = androidUnityLibJavaClass.CallStatic<AndroidJavaObject>("getInstance");
    }

    public void ConnectToArduino()
    {
        if (_androidInstanceJavaObject != null)
        {
            _androidInstanceJavaObject.Call("TerminalFragment");
        }
        else
        {
            Debug.LogError("Error, android native library Java object is null!!!");
        }
    }
}