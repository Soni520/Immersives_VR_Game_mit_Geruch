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
            if(!ObjectScentManager.MenuOn)
            {
                ObjectScentManager.MenuOn = true;
                MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 1.75f);
                SearchingObject.GetComponent<CanvasGroup>().alpha = 0;
                
            } else if(ObjectScentManager.MenuOn)
            {
                ObjectScentManager.MenuOn = false;
                SearchingObject.GetComponent<CanvasGroup>().alpha = 1;
                MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, -2f);
            }
        }
    }

    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void GoBack()
    {
        ObjectScentManager.MenuOn = false;
        SearchingObject.GetComponent<CanvasGroup>().alpha = 1;
        MenuCanvas.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, -2f);
    }
}
