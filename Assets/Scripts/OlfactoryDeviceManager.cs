using TMPro;
using UnityEngine;

/*
 * This script manages the communication with a the olfactory device connected to the Meta Quest.
 */
public class OlfactoryDeviceManager : MonoBehaviour
{
    protected AndroidJavaObject _androidInstanceJavaObject;
    
    [SerializeField] private int _baudRate; // Speed of data transmission
    [SerializeField] private int _dataBits; // Number of bits per message
    [SerializeField] private int _stopBits; // Signal bits at the end of a character
    [SerializeField] private int _parity;   // Error-checking bit
    [SerializeField] private TextMeshProUGUI TextField;


    private void Awake()
    {
        // Create the Java Class Object
        AndroidJavaObject androidUnityLibJavaClass = new AndroidJavaObject("com.ethanlin.serialportlib.UnitySerialPortDataLib");
        // Initialize the Java Class Object
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

    // Opens the communication channel to the olfactory device
    public void Open()
    {
        if (_androidInstanceJavaObject != null)
        {
            // Call the Java method to open the port
            _androidInstanceJavaObject.Call("openSerialPort", _baudRate, _dataBits, _stopBits, _parity);
            TextField.text = "Connected";
        }
        else
        {
            Debug.LogError("Error, android native library Java object is null");
        }
    }

    // Sends a string command (e.g. setAPump...) to the device via the serial port
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

    // Selects a specific pump and sets a default frequency
    public void SetPump(int pump) 
    {
        Write("setAPump:" + pump);
        Write("setF:75");
    }

    // Set the frequency of the scent release
    public void SetFrequency(double frequency)
    {
        Write("setF:" + frequency);
    }

    // Activate current pump
    public bool StartPump()
    {
        Write("setStatus:1");
        return true;
    }

    // Iterates through all possible pumps (0-4) and shuts them down
    public bool StopAllPumps() 
    {
        for (int i = 0; i < 5; i++)
        {
            Write("setAPump:" + i);
            Write("setStatus:0");   // Stop pump i
        }
        return false;
    }

    // Turn off all pumps when app is closed
    void OnApplicationQuit()
    {
        StopAllPumps();
    }
}