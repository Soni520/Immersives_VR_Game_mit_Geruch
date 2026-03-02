using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/*
 * This script manages the video clip for the scene
 */
public class VideoPlayerManager : MonoBehaviour
{
    [Header("Drag the video clips in the right order here")]
    // Array to hold the 4 video clips
    public VideoClip[] VideoClips = new VideoClip[4];
    // Stores the numeric ID of the selected fruit
    private int Fruit = 0;
    void Start()
    {
        // Assign the clip based on the index.
        Fruit = PlayerPrefs.GetInt("CurrentFruit", Random.Range(1, 4));
        GetComponent<VideoPlayer>().clip = VideoClips[Fruit - 1];
    }
}
