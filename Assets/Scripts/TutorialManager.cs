using UnityEngine;
using TMPro;
using Meta.XR.BuildingBlocks;
using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using Unity.VisualScripting;
using System.Security.Cryptography;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;


/*
 * This script manages the step-by-step tutorial
 */
public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Canvas TutorialCanvas;
    [SerializeField] private TextMeshProUGUI TutorialText;
    [SerializeField] private TextMeshProUGUI ContinueText;
    [SerializeField] private Canvas SearchingObjectCanvas;
    private search_logic SearchLogic;
    private int TutorialStage = -1; // current step of tutorial

    void Awake()
    {
        SearchLogic = GetComponent<search_logic>();
    }

    void Update()
    {
        // Check for the "B" button press
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            TutorialStage++;
            switch (TutorialStage)
            {
                case 0: // Controls explanation
                    TutorialText.text = "\nUse the <b>Left Joystick</b> to <b>walk</b> around in the world and the <b>Right Joystick</b> to <b>look</b> around.\n\nIf you want to <b>pause</b> or get back to the <b>Menu</b> press the <b>Menu Button</b> on your left controller.";
                    break;
                case 1: // Hint explanation
                    TutorialText.text = "\nThe hint in the <b>Upper Left Corner</b> shows you which <b>Fruit</b> to look for.";
                    // Show the searching text
                    SearchingObjectCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-1.093f, 0.552f, 3);
                    SearchingObjectCanvas.GetComponentInChildren<Image>().color = new Color(1, 1, 0, 1);
                    // Reposition and resize the explanation text
                    TutorialCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0.5f, -0.35f, 3);
                    TutorialCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 70);
                    break;
                case 2: // Particle explanation
                    TutorialText.text = "\nIf you aren't getting closer after some time, <b>Particles</b> will appear beneath your feet to <b>point the way</b>. These act as a guide toward the fruit you are searching for. Once you begin walking in the <b>Right Direction</b>, the trail will <b>disappear</b>.";
                    SearchingObjectCanvas.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 200/255f);
                    TutorialCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0.6f, -0.2f, 3);
                    TutorialCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 90);
                    // Show particle
                    SearchLogic.timeUntilHint = 0;
                    break;
                case 3: //  Meditation explanation
                    TutorialText.text = "Once you find the fruit, you will be taken to a <b>Medition World</b>. There you can do some <b>Breating Excercises</b>, <b>meditate</b> and <b>relax</b>.";
                    TutorialCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(135, 65);
                    // Show particle after 10 seconds
                    SearchLogic.timeUntilHint = 10;
                    break;
                case 4: // Ready to find the fruit
                    TutorialText.text = "Now try to find the fruit.";
                    ContinueText.text = "Press B to hide this window.";
                    TutorialCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0f, -0.2f, 3);
                    TutorialCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(135, 35);
                    break;
                case 5: // Hide tutorial
                    TutorialCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0.8f, -0.2f, -2.5f);
                    break;
                default:
                    break;
            }
        }

        // If the player finds the fruit and tutorial is advanced
        if (SearchLogic.showRays && TutorialStage >= 4)
        {
            TutorialText.text = "Proceed to the <b>meditation</b> by aiming at the <b>fruit</b> and using the <b>trigger</b>.";
            ContinueText.text = "";
            TutorialCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-0.45f, 0.35f, 3);
            TutorialCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(105, 35);
            SearchingObjectCanvas.transform.position = new Vector3(-1.093f, 0.552f, -3);
        }
    }
}
