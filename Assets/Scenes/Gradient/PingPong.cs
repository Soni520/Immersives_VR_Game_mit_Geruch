using UnityEngine;
using UnityEngine.Video;

public class PingPongVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public float playbackSpeed = 1f;

    private bool forward = true;

    void Start()
    {
        videoPlayer.isLooping = false;
        videoPlayer.playOnAwake = false;
        videoPlayer.Play();
    }

    void Update()
    {
        if (!videoPlayer.isPrepared)
            return;

        double delta = Time.deltaTime * playbackSpeed;

        if (forward)
        {
            videoPlayer.time += delta;

            if (videoPlayer.time >= videoPlayer.length)
            {
                videoPlayer.time = videoPlayer.length;
                forward = false;
            }
        }
        else
        {
            videoPlayer.time -= delta;

            if (videoPlayer.time <= 0)
            {
                videoPlayer.time = 0;
                forward = true;
            }
        }
    }
}
