using System;
using TMPro;
using UnityEngine;

public class ObjectScentManager : MonoBehaviour
{
    private search_logic SearchLogicScript;
    private OlfactoryDeviceManager OlfactoryDeviceManager;
    [SerializeField] private GameObject Player;
    private string CurrentPump = null;
    private double CurrentFrequency = -1.0;
    [SerializeField] private TextMeshProUGUI TextField;
    [SerializeField] private TextMeshProUGUI FruitField;
    public bool MenuOn = false;
    private bool PumpOn = false;
    private float MaxScentValue = -1f;
    private double RoundValue = 10.0;

    void Awake()
    {
        SearchLogicScript = GetComponent<search_logic>();
        OlfactoryDeviceManager = GetComponent<OlfactoryDeviceManager>();
        MaxScentValue = PlayerPrefs.GetInt("ScentIntensity", 50) * 3.0f;
        RoundValue = PlayerPrefs.GetInt("ScentIntensity", 50) / 5.0f;
    }

    void Start()
    {

    }

    void Update()
    {
        if (!MenuOn)
        {
            var FindNearestObject = NearestObject();
            string NewPump = FindNearestObject.Item1.name;
            double FrequencyMultiplier = 500 / MaxScentValue;
            double NewFrequency = Math.Round((-FindNearestObject.Item2 / FrequencyMultiplier + MaxScentValue) / RoundValue) * RoundValue;

            if (CurrentPump != NewPump)
            {
                SetScent(NewPump);
                CurrentPump = NewPump;
                SetFrequency(NewFrequency);
                CurrentFrequency = NewFrequency;
            }
            if (CurrentFrequency != NewFrequency)
            {
                SetFrequency(NewFrequency);
                CurrentFrequency = NewFrequency;
            }
        }
        if (MenuOn && PumpOn)
        {
            PumpOn = OlfactoryDeviceManager.StopAllPumps();
            CurrentPump = null;
        }
        
        TextField.text = CurrentPump + ", Frequency: " + CurrentFrequency.ToString() + "MaxScentValue: " + MaxScentValue.ToString() + "Distance: " + NearestObject().Item2.ToString();
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
        switch (Pump)
        {
            case "Watermelon(Clone)":
                OlfactoryDeviceManager.SetPump(1);
                FruitField.text = "Find the Watermelon";
                break;
            case "Orange(Clone)":
                OlfactoryDeviceManager.SetPump(2);
                FruitField.text = "Find the Orange";
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
        if (0 <= Frequency && Frequency <= MaxScentValue)
        {
            OlfactoryDeviceManager.SetFrequency(Frequency);
        }else
        {
            OlfactoryDeviceManager.SetFrequency(0);
        }
        TextField.text = CurrentPump + ", Frequency: " + Frequency.ToString() + "MaxScentValue: " + MaxScentValue.ToString() + "Distance: " + NearestObject().Item2.ToString();
    }
}
