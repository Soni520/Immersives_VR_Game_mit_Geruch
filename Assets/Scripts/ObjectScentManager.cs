using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;

public class ObjectScentManager : MonoBehaviour
{
    private search_logic SearchLogicScript;
    private OlfactoryDeviceManager OlfactoryDeviceManager;
    private GameObject Player;
    void Awake()
    {
        SearchLogicScript = GetComponent<search_logic>();
        OlfactoryDeviceManager = GetComponent<OlfactoryDeviceManager>();
        Player = GameObject.Find("PlayerController");
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (SearchLogicScript != null && Player != null)
        {
            var FindNearestObject = NearestObject();
            
        }else
        {
            SearchLogicScript = GetComponent<search_logic>();
            Player = GameObject.Find("PlayerController");
        }
    }

    private (GameObject, float) NearestObject()
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
        return (ReturnObject, NearestDistance);
    }

    private void SetScent(GameObject GameObject)
    {

        OlfactoryDeviceManager.SetPump();
    }

    private void SetFrequency(float Distance)
    {

        OlfactoryDeviceManager.SetFrequency();
        OlfactoryDeviceManager.StartPump();
    }
}
