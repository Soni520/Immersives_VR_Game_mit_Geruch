using UnityEngine;

public class MenuManager : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().interactable = !GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().interactable;
            if(GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().alpha == 0)
            {
                GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().alpha = 1;
            } else if(GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().alpha == 1)
            {
                GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
            }
        }
    }

    public void GoBack()
    {
        GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("MenuCanvas").GetComponent<CanvasGroup>().interactable = false;

    }

    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
