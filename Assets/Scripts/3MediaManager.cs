using UnityEngine;
using UnityEngine.Video;

public class MediaManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;

    public VideoClip[] videos;
    public AudioClip[] musics;

    void Start()
    {
        int id = PrefabData.foundPrefabID;

        if (id < 0 || id >= videos.Length)
        {
            Debug.LogError("Ungültige Prefab ID!");
            return;
        }

        videoPlayer.clip = videos[id];
        audioSource.clip = musics[id];

        videoPlayer.Play();
        audioSource.Play();
    }
}
