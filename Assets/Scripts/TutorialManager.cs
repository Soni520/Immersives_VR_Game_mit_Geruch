using UnityEngine;
using TMPro;
using Meta.XR.BuildingBlocks;
using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using Unity.VisualScripting;
using System.Security.Cryptography;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Canvas TutorialCanvas;
    [SerializeField] private TextMeshProUGUI TutorialText;
    [SerializeField] private TextMeshProUGUI ContinueText;
    [SerializeField] private Canvas SearchingObjectCanvas;
    private search_logic SearchLogic;
    private int TutorialStage = -1;

    void Awake()
    {
        SearchLogic = GetComponent<search_logic>();
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            TutorialStage++;
            switch (TutorialStage)
            {
                case 0:
                    TutorialText.text = "\nUse the <b>Left Joystick</b> to <b>walk</b> around in the world and the <b>Right Joystick</b> to <b>look</b> around.\n\nIf you want to <b>pause</b> or get back to the <b>menu</b> press the <b>menu button</b> on your left controller.";
                    break;
                case 1:
                    TutorialText.text = "\nThe hint in the <b>Upper Left Corner</b> shows you which <b>Fruit</b> to look for.";
                    SearchingObjectCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-1.093f, 0.552f, 3);
                    SearchingObjectCanvas.GetComponentInChildren<Image>().color = new Color(1, 1, 0, 1);
                    TutorialCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0.5f, -0.35f, 3);
                    TutorialCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 70);
                    break;
                case 2:
                    TutorialText.text = "\nIf you aren't getting closer after some time, <b>particles</b> will appear beneath your feet to <b>point the way</b>. These act as a guide toward the Fruit you are searching for. Once you begin walking in the <b>right direction</b>, the trail will <b>disappear</b>.";
                    SearchingObjectCanvas.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 200/255f);
                    TutorialCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0.6f, -0.2f, 3);
                    TutorialCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 90);
                    SearchLogic.timeUntilHint = 0;
                    break;
                case 3:
                    TutorialText.text = "Now try to find the fruit.";
                    ContinueText.text = "Press B to hide this window.";
                    SearchLogic.timeUntilHint = 10;
                    TutorialCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0f, -0.2f, 3);
                    TutorialCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(135, 35);
                    break;
                case 4:
                    TutorialCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0.8f, -0.2f, -2.5f);
                    break;
                default:
                    break;
            }
        }
        if (SearchLogic.showRays && TutorialStage >= 3)
        {
            TutorialText.text = "Proceed to the <b>meditation</b> by aiming at the <b>fruit</b> and using the <b>trigger</b>.";
            ContinueText.text = "";
            TutorialCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-0.45f, 0.35f, 3);
            TutorialCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(105, 35);
            SearchingObjectCanvas.transform.position = new Vector3(-1.093f, 0.552f, -3);
        }
    }
}
