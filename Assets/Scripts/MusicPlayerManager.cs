using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/*
 * This script manages the background music for the scene
 */
public class MusicPlayerManager : MonoBehaviour
{
    [Header("Drag the music clips in the right order here")]
    // Array to hold the 4 music tracks
    public AudioClip[] AudioClips = new AudioClip[4];
    // Stores the numeric ID of the selected fruit
    private int Fruit = 0;
    void Start()
    {
        // Assign the clip based on the index.
        Fruit = PlayerPrefs.GetInt("CurrentFruit", Random.Range(1, 4));
        GetComponent<AudioSource>().clip = AudioClips[Fruit - 1];

        // Start the music
        GetComponent<AudioSource>().Play();
    }
}
