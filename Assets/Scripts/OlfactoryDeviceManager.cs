using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OlfactoryDeviceManager : MonoBehaviour
{
    protected AndroidJavaObject _androidInstanceJavaObject;
    
    [SerializeField] private int _baudRate;
    [SerializeField] private int _dataBits;
    [SerializeField] private int _stopBits;
    [SerializeField] private int _parity;


    private void Awake()
    {
        AndroidJavaObject androidUnityLibJavaClass = new AndroidJavaObject("com.ethanlin.serialportlib.UnitySerialPortDataLib");
        _androidInstanceJavaObject = androidUnityLibJavaClass.CallStatic<AndroidJavaObject>("getInstance");
        if (_androidInstanceJavaObject != null)
        {
            _androidInstanceJavaObject.Call("initSerialPortManagerAndReceiver");
        }
        else
        {
            Debug.LogError("Error, android native library Java object is null");
        }
    }

    private void Start()
    {
        Open();
        Open();

    }

    public void Open()
    {
        if (_androidInstanceJavaObject != null)
        {
            _androidInstanceJavaObject.Call("openSerialPort", _baudRate, _dataBits, _stopBits, _parity);
        }
        else
        {
            Debug.LogError("Error, android native library Java object is null");
        }
    }

    public void Write(string message)
    {
        if (_androidInstanceJavaObject != null)
        {
            _androidInstanceJavaObject.Call("writeSerialPort", message);
        }
        else
        {
            Debug.LogError("Error, android native library Java object is null");
        }
    }

    public void SetPump(int pump) 
    {
        Write("setAPump:" + pump);
        Write("setF:75");
    }

    public void SetFrequency(double frequency)
    {
        Write("setF:" + frequency);
    }
    public void StartPump()
    {
        Write("setStatus:1");
    }

    public void StopAllPumps() 
    {
        for (int i = 0; i < 5; i++)
        {
            Write("setAPump:" + i);
            Write("setStatus:0");
        }
    }
}
