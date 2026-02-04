using System.Security.Cryptography;
using UnityEngine;
using TMPro;

public class WorldMenuManager : MonoBehaviour
{
    private ObjectScentManager ObjectScentManager;

    private void Awake()
    {
        ObjectScentManager = GetComponent<ObjectScentManager>();
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().interactable = !GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().interactable;
            GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().blocksRaycasts = !GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().blocksRaycasts;
            if(GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().alpha == 0)
            {
                GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().alpha = 1;
                ObjectScentManager.MenuOn = true;
                GameObject.Find("SearchingObject").GetComponent<CanvasGroup>().alpha = 0;
                
            } else if(GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().alpha == 1)
            {
                GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
                ObjectScentManager.MenuOn = false;
                GameObject.Find("SearchingObject").GetComponent<CanvasGroup>().alpha = 1;
                
            }
        }
    }

    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void GoBack()
    {
        GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().blocksRaycasts = false;
        ObjectScentManager.MenuOn = false;
        GameObject.Find("SearchingObject").GetComponent<CanvasGroup>().alpha = 1;
    }
}
