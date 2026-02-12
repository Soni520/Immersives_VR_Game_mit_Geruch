using UnityEngine;
using TMPro;
using Meta.XR.BuildingBlocks;
using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using Unity.VisualScripting;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Canvas TutorialCanvas;
    [SerializeField] private TextMeshProUGUI TutorialText;
    [SerializeField] private Canvas SearchingObject;
    private int TutorialStage = -1;

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
                    SearchingObject.transform.position = new Vector3(-1.093f, 0.552f, 3);
                    SearchingObject.GetComponentInChildren<Image>().Color = new Color(1, 1, 0, 1);
                    TutorialCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0.5f, -0.35f, 1.75f);
                    TutorialCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 70);
                    TutorialText.text = "\nThe hint in the <b>Upper Left Corner</b> shows you which <b>Fruit</b> to look for.";
                    break;
                case 2:
                    SearchingObject.GetComponentInChildren<Image>().Color = new Color(1, 1, 1, 200/255f);
                    TutorialText.text = "";
                    break;
                case 3:
                    TutorialText.text = "";
                    break;
                case 4:
                    TutorialText.text = "";
                    break;
                default:
                    TutorialCanvas.gameObject.SetActive(false);
                    break;
            }
        }
    }
}
