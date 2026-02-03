using UnityEngine;
using UnityEngine.Video;

public class PingPongVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    private bool forward = true;

    void Start()
    {
        videoPlayer.isLooping = false; // wichtig!
        videoPlayer.loopPointReached += OnVideoEnd;

        PlayForward();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        forward = !forward;

        if (forward)
            PlayForward();
        else
            PlayBackward();
    }

    void PlayForward()
    {
        videoPlayer.playbackSpeed = 1f;
        videoPlayer.time = 0;
        videoPlayer.Play();
    }

    void PlayBackward()
    {
        videoPlayer.playbackSpeed = -1f;
        videoPlayer.time = videoPlayer.length;
        videoPlayer.Play();
    }
}
