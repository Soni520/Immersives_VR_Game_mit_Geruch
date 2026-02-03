using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public List<VideoClip> videoClips;
    private int currentClipIndex = 0;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
        PlayCurrentVideo();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        currentClipIndex = (currentClipIndex + 1) % videoClips.Count;
        PlayCurrentVideo();
    }

    void PlayCurrentVideo()
    {
        videoPlayer.clip = videoClips[currentClipIndex];
        videoPlayer.Play();
    }
}
