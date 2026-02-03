using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OlfactoryDeviceManager : MonoBehaviour
{
    protected AndroidJavaObject _androidInstanceJavaObject;
    
    [SerializeField] private int _baudRate;
    [SerializeField] private int _dataBits;
    [SerializeField] private int _stopBits;
    [SerializeField] private int _parity;
    [SerializeField] private TextMeshProUGUI TextField;


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

    }

    public void Open()
    {
        if (_androidInstanceJavaObject != null)
        {
            _androidInstanceJavaObject.Call("openSerialPort", _baudRate, _dataBits, _stopBits, _parity);
            TextField.text = "Connected";
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
    public bool StartPump()
    {
        Write("setStatus:1");
        return true;
    }

    public void StartTest()
    {
        Write("setStatus:1");
    }

    public bool StopAllPumps() 
    {
        for (int i = 0; i < 5; i++)
        {
            Write("setAPump:" + i);
            Write("setStatus:0");
        }
        return false;
    }

    void OnApplicationQuit()
    {
        StopAllPumps();
    }

    public void TestFrequency()
    {
        SetFrequency(PlayerPrefs.GetFloat("ScentIntensity", 1.0f) * 3.0);
    }
}