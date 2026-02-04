using UnityEngine;
using UnityEngine.Video;

public class VideoPingPongDual : MonoBehaviour
{
    public VideoPlayer forward;
    public VideoPlayer backward;

    void Start()
    {
        forward.loopPointReached += OnForwardEnd;
        backward.loopPointReached += OnBackwardEnd;

        forward.Play();
    }

    void OnForwardEnd(VideoPlayer vp)
    {
        forward.Stop();
        backward.Play();
    }

    void OnBackwardEnd(VideoPlayer vp)
    {
        backward.Stop();
        forward.Play();
    }
}
