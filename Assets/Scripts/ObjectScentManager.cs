using System;
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
    [SerializeField] private TextMeshProUGUI FruitField;
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
            double NewFrequency = Math.Round((-FindNearestObject.Item2 / 3.4 + 150) / 10) * 10;

            if (CurrentPump != NewPump)
            {
                SetScent(NewPump);
                CurrentPump = NewPump;
            }
            if (CurrentFrequency != NewFrequency)
            {
                SetFrequency(NewFrequency);
                CurrentFrequency = NewFrequency;
            }
            TextField.text = CurrentPump + ", Frequency: " + CurrentFrequency.ToString() + ", Distance: " + FindNearestObject.Item2.ToString();
        }
        if (OlfactoryDeviceManager != null && MenuOn && PumpOn)
        {
            PumpOn = OlfactoryDeviceManager.StopAllPumps();
            CurrentPump = null;
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
                FruitField.text = "Find the Watermelon";
                break;
            case "Coconut(Clone)":
                OlfactoryDeviceManager.SetPump(2);
                FruitField.text = "Find the Coconut";
                break;
            case "Lemon(Clone)":
                OlfactoryDeviceManager.SetPump(3);
                FruitField.text = "Find the Lemon";
                break;
            case "Pineapple(Clone)":
                OlfactoryDeviceManager.SetPump(4);
                FruitField.text = "Find the Pineapple";
                break;

        }
        PumpOn = OlfactoryDeviceManager.StartPump();
    }

    private void SetFrequency(double Frequency)
    {
        if (0 <= Frequency && Frequency <= 150)
        {
            OlfactoryDeviceManager.SetFrequency(Frequency);
        }else
        {
            OlfactoryDeviceManager.SetFrequency(0);
        }
    }
}
