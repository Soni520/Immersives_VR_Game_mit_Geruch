using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MusicPlayerManager : MonoBehaviour
{
    [Header("Drag the music clips in the right order here")]
    public AudioClip[] AudioClips = new AudioClip[4];
    private int Fruit = 0;
    void Start()
    {   
        Fruit = PlayerPrefs.GetInt("CurrentFruit", Random.Range(1, 4));
        GetComponent<AudioSource>().clip = AudioClips[Fruit - 1];
        GetComponent<AudioSource>().Play();
    }
}
