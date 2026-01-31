using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;

public class ObjectScentManager : MonoBehaviour
{
    [SerializeField] private search_logic SearchLogicScript;
    [SerializeField] private OlfactoryDeviceManager OlfactoryDeviceManager;
    [SerializeField] private GameObject Player;
    private string CurrentPump = null;
    private double CurrentFrequency = -1.0;
    [SerializeField] private TextMeshProUGUI TextField;

    void Awake()
    {

    }

    void Start()
    {
        
    }

    void Update()
    {
        if (SearchLogicScript != null && Player != null)
        {
            var FindNearestObject = NearestObject();
            string NewPump = FindNearestObject.Item1.name;
            double NewFrequency = (-FindNearestObject.Item2 / 5 + 100);

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
        OlfactoryDeviceManager.StartPump();
    }
}
