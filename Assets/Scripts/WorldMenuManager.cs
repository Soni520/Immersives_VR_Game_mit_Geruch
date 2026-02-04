using System.Security.Cryptography;
using UnityEngine;
using TMPro;

public class WorldMenuManager : MonoBehaviour
{
    private ObjectScentManager ObjectScentManager;
    private GameObject MenuCanvas;
    private GameObject SearchingObject;

    private void Awake()
    {
        ObjectScentManager = GetComponent<ObjectScentManager>();
        MenuCanvas = GameObject.Find("MenuCanvas");
        SearchingObject = GameObject.Find("SearchingObject");
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            //GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().interactable = !GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().interactable;
            //GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().blocksRaycasts = !GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().blocksRaycasts;
            if(!ObjectScentManager.MenuOn)
            {
                //GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().alpha = 1;
                //GameObject.Find("ISDK_RayCanvasInteraction_Menu").GetComponent<CanvasGroup>().enabled = true;
                ObjectScentManager.MenuOn = true;
                MenuCanvas.GetComponent<RectTransform>().position = new Vector3(0, 0, 1.75f);
                SearchingObject.GetComponent<CanvasGroup>().alpha = 0;
                
            } else if(ObjectScentManager.MenuOn)
            {
                //GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
                //GameObject.Find("ISDK_RayCanvasInteraction_Menu").GetComponent<CanvasGroup>().enabled = false;
                ObjectScentManager.MenuOn = false;
                SearchingObject.GetComponent<CanvasGroup>().alpha = 1;
                MenuCanvas.GetComponent<RectTransform>().position = new Vector3(0, 0, -2f);
            }
        }
    }

    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void GoBack()
    {
        //GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        //GameObject.Find("ISDK_RayCanvasInteraction_Menu").GetComponent<CanvasGroup>().enabled = false;
        //GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().interactable = false;
        //GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().blocksRaycasts = false;
        ObjectScentManager.MenuOn = false;
        SearchingObject.GetComponent<CanvasGroup>().alpha = 1;
        MenuCanvas.GetComponent<RectTransform>().position = new Vector3(0, 0, -2f);
    }
}
