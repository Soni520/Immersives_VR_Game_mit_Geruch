using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour
{
    [Header("Drag the video clips in the right order here")]
    public VideoClip[] VideoClips = new VideoClip[4];
    private int Fruit = 0;
    void Start()
    {   
        Fruit = PlayerPrefs.GetInt("CurrentFruit", Random.Range(1, 4));
        GetComponent<VideoPlayer>().clip = VideoClips[Fruit - 1];
    }
}
