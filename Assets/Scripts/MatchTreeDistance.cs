using UnityEngine;

public class MatchTreeDistance : MonoBehaviour
{
    public int targetLayer = 6;
    public float distance = 2000f;

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        float[] distances = new float[32];

        distances[targetLayer] = distance;

        cam.layerCullDistances = distances;
        cam.layerCullSpherical = true;
    }
}