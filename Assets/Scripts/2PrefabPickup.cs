using UnityEngine;
using UnityEngine.SceneManagement;

public class PrefabPickup : MonoBehaviour
{
    [Range(0, 3)]
    public int prefabID;
    public string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PrefabData.foundPrefabID = prefabID;
            SceneManager.LoadScene("Gradient");
        }
    }
}
