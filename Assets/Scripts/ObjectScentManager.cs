using System;
using TMPro;
using UnityEngine;

/*
 * This script calculates the player's distance to nearby fruit objects and
 * updates the olfactory device to change scent types and intensities dynamically.
 */
public class ObjectScentManager : MonoBehaviour
{
    private search_logic SearchLogicScript;
    private OlfactoryDeviceManager OlfactoryDeviceManager;
    [SerializeField] private GameObject Player;
    private string CurrentPump = null;
    private double CurrentFrequency = -1.0;
    [SerializeField] private TextMeshProUGUI TextField;     // Debug window
    [SerializeField] private TextMeshProUGUI FruitField;    // Hint for player
    public bool MenuOn = false;
    private bool PumpOn = false;
    private float MaxScentValue = -1f;
    private double RoundValue = 10.0;

    void Awake()
    {
        SearchLogicScript = GetComponent<search_logic>();
        OlfactoryDeviceManager = GetComponent<OlfactoryDeviceManager>();

        // Get choosen scent intensity
        MaxScentValue = PlayerPrefs.GetInt("ScentIntensity", 50) * 3.0f;
        RoundValue = PlayerPrefs.GetInt("ScentIntensity", 50) / 5.0f;
    }

    void Start()
    {

    }

    void Update()
    {
        // Only update scent logic if the menu is closed
        if (!MenuOn)
        {
            // Identify the closest fruit and how far away it is 
            var FindNearestObject = NearestObject();
            string NewPump = FindNearestObject.Item1.name;

            // Math to convert distance into a frequency value for the pump 
            double FrequencyMultiplier = 500 / MaxScentValue;
            double NewFrequency = Math.Round((-FindNearestObject.Item2 / FrequencyMultiplier + MaxScentValue) / RoundValue) * RoundValue;

            // Update the pump if the nearest object has changed
            if (CurrentPump != NewPump)
            {
                SetScent(NewPump);
                CurrentPump = NewPump;
                SetFrequency(NewFrequency);
                CurrentFrequency = NewFrequency;
            }
            // Update the intensity if the player moves in the direction of the fruit
            if (CurrentFrequency != NewFrequency)
            {
                SetFrequency(NewFrequency);
                CurrentFrequency = NewFrequency;
            }
        }
        // Stops all pumps, if menu is open
        if (MenuOn && PumpOn)
        {
            PumpOn = OlfactoryDeviceManager.StopAllPumps();
            CurrentPump = null;
        }

        // Debug display for testing
        TextField.text = CurrentPump + ", Frequency: " + CurrentFrequency.ToString() + "MaxScentValue: " + MaxScentValue.ToString() + "Distance: " + NearestObject().Item2.ToString();
    }

    /*
     * Loops through all spawned fruit objects to find the one closest to the player.
     * Returns the GameObject and the distance as a Tuple.
     */
    private (GameObject, double) NearestObject()
    {
        GameObject ReturnObject = null;
        float NearestDistance = float.MaxValue;
        foreach (GameObject gameObject in SearchLogicScript.spawnedObjects)
        {
            // Calculate the distance between fruit and player
            float TempDistance = Vector3.Distance(gameObject.transform.position, Player.transform.position);
            
            // If this fruit is closer than the one we previously checked, update our "nearest" fruit
            if (TempDistance < NearestDistance)
            {
                ReturnObject = gameObject;
                NearestDistance = TempDistance;
            }
        }
        return (ReturnObject, (double)NearestDistance);
    }

    /*
     * Maps the fruit to a pump ID.
     * 1-Watermelon, 2-Orange, 3-Lemon, 4-Pineapple
     */
    private void SetScent(string Pump)
    {
        switch (Pump)
        {
            case "Watermelon(Clone)":
                OlfactoryDeviceManager.SetPump(1);
                PlayerPrefs.SetInt("CurrentFruit", 1);
                FruitField.text = "Find the Watermelon";
                break;
            case "Orange(Clone)":
                OlfactoryDeviceManager.SetPump(2);
                PlayerPrefs.SetInt("CurrentFruit", 2);
                FruitField.text = "Find the Orange";
                break;
            case "Lemon(Clone)":
                OlfactoryDeviceManager.SetPump(3);
                PlayerPrefs.SetInt("CurrentFruit", 3);
                FruitField.text = "Find the Lemon";
                break;
            case "Pineapple(Clone)":
                OlfactoryDeviceManager.SetPump(4);
                PlayerPrefs.SetInt("CurrentFruit", 4);
                FruitField.text = "Find the Pineapple";
                break;

        }
        // Start scent
        PumpOn = OlfactoryDeviceManager.StartPump();
    }

    /*
     * Updates the frequency of the scent release based on the choosen intensity
     */
    private void SetFrequency(double Frequency)
    {
        if (0 <= Frequency && Frequency <= MaxScentValue)
        {
            OlfactoryDeviceManager.SetFrequency(Frequency);
        }else
        {
            // Turn off scent if the player is too far away
            OlfactoryDeviceManager.SetFrequency(0);
        }

        // Debug display for testing
        TextField.text = CurrentPump + ", Frequency: " + Frequency.ToString() + "MaxScentValue: " + MaxScentValue.ToString() + "Distance: " + NearestObject().Item2.ToString();
    }
}
