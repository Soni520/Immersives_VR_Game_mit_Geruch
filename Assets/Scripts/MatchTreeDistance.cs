using UnityEngine;

/*
 * This script sets a custom draw distance for a specific layer, 
 * allowing the fruits to be culled sooner like the trees in the world
 */
public class MatchTreeDistance : MonoBehaviour
{
    // The rendering layer index to apply the culling to
    public int targetLayer = 6;

    // The maximum distance at which the objects remain visible
    public float distance = 2000f;

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        float[] distances = new float[32];

        // Assign the custom culling distance to the specific target layer
        distances[targetLayer] = distance;

        cam.layerCullDistances = distances;
        cam.layerCullSpherical = true;
    }
}