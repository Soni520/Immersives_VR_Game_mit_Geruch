using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectScentManager : MonoBehaviour
{
    private search_logic SearchLogicScript;
    private OlfactoryDeviceManager OlfactoryDeviceManager;
    [SerializeField] private GameObject Player;
    private string CurrentPump = null;
    private double CurrentFrequency = -1.0;
    [SerializeField] private TextMeshProUGUI TextField;
    private bool OlfactoryStarted = false;
    public bool MenuOn = false;
    private bool PumpOn = false;

    void Awake()
    {
        SearchLogicScript = GetComponent<search_logic>();
        OlfactoryDeviceManager = GetComponent<OlfactoryDeviceManager>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (OlfactoryDeviceManager != null && SearchLogicScript != null && Player != null && !MenuOn)
        {
            var FindNearestObject = NearestObject();
            string NewPump = FindNearestObject.Item1.name;
            double NewFrequency = (-FindNearestObject.Item2 / 5 + 100);
            TextField.text = "after new frequency";

            if (CurrentPump != NewPump)
            {
                TextField.text = "before setscent";
                SetScent(NewPump);
                TextField.text = "after setscent";
                CurrentPump = NewPump;
            }
            if (CurrentFrequency != NewFrequency)
            {
                TextField.text = "before set frequency";
                SetFrequency(NewFrequency);
                TextField.text = "after set frequency";
                CurrentFrequency = NewFrequency;
            }
            TextField.text = CurrentPump + ", Frequency: " + CurrentFrequency.ToString() + ", Distance: " + FindNearestObject.Item2.ToString();
        }
        if (OlfactoryDeviceManager != null && MenuOn && PumpOn)
        {
            PumpOn = OlfactoryDeviceManager.StopAllPumps();
        }
    }

    private (GameObject, double) NearestObject()
    {
        GameObject ReturnObject = null;
        float NearestDistance = float.MaxValue;
        foreach (GameObject gameObject in SearchLogicScript.spawnedObjects)
        {
            float TempDistance = Vector3.Distance(gameObject.transform.position, Player.transform.position);
            if (TempDistance < NearestDistance)
            {
                ReturnObject = gameObject;
                NearestDistance = TempDistance;
            }
        }
        return (ReturnObject, (double)NearestDistance);
    }

    private void SetScent(string Pump)
    {
        if (!OlfactoryStarted)
        {
            OlfactoryDeviceManager.Open();
            OlfactoryDeviceManager.Open();
            OlfactoryStarted = true;
        }

        switch (Pump)
        {
            case "Watermelon(Clone)":
                OlfactoryDeviceManager.SetPump(1);
                break;
            case "Coconut(Clone)":
                OlfactoryDeviceManager.SetPump(2);
                break;
            case "Lemon(Clone)":
                OlfactoryDeviceManager.SetPump(3);
                break;
            case "Pineapple(Clone)":
                OlfactoryDeviceManager.SetPump(4);
                break;

        }
    }

    private void SetFrequency(double Frequency)
    {
        if (0 < Frequency && Frequency < 100)
        {
            OlfactoryDeviceManager.SetFrequency(Frequency);
        }else
        {
            OlfactoryDeviceManager.SetFrequency(0);
        }
        PumpOn = OlfactoryDeviceManager.StartPump();
    }
}
