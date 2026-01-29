using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;

public class ObjectScentManager : MonoBehaviour
{
    private search_logic SearchLogicScript;
    private OlfactoryDeviceManager OlfactoryDeviceManager;
    [SerializeField] private GameObject Player;
    private string CurrentPump = null;
    private double CurrentFrequency = -1.0;
    [SerializeField] private TextMeshProUGUI TextField;

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
        if (SearchLogicScript != null && Player != null)
        {
            var FindNearestObject = NearestObject();
            string NewPump = FindNearestObject.Item1.name;
            double NewFrequency = (-FindNearestObject.Item2 / 3.5 + 200);

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
        foreach (GameObject gameObject in SearchLogicScript.spawnableObjects)
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
            case "Watermelon":
                OlfactoryDeviceManager.SetPump(1);
                break;
            case "Lemon":
                OlfactoryDeviceManager.SetPump(2);
                break;
            case "Pineapple":
                OlfactoryDeviceManager.SetPump(3);
                break;
            case "Coconut":
                OlfactoryDeviceManager.SetPump(4);
                break;

        }
    }

    private void SetFrequency(double Frequency)
    {
        if (Frequency > 0)
        {
            OlfactoryDeviceManager.SetFrequency(Frequency);
        }else
        {
            OlfactoryDeviceManager.SetFrequency(0);
        }
        OlfactoryDeviceManager.StartPump();
    }
}
