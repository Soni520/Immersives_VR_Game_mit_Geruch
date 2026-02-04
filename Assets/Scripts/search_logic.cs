using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using Random = UnityEngine.Random;
using JetBrains.Annotations;

public class search_logic : MonoBehaviour
{

    public TextMeshProUGUI uiText;
    [SerializeField] private GameObject Player;

    public List<Vector3> spawnPositions;
    public List<GameObject> spawnableObjects;

    [Header("Hint Settings")]
    public float timeUntilHint = 10f;
    public ParticleSystem particleTrailPrefab;

    [Header("VR Controller Settings")]
    public OVRInput.Controller rightController = OVRInput.Controller.RTouch;
    public OVRInput.Controller leftController = OVRInput.Controller.LTouch;
    public float rayDistance = 10f;
    public LayerMask raycastLayerMask = ~0; // Alle Layer

    [Header("Scene Settings")]
    public string targetSceneName = "meditation_scene";

    public List<GameObject> spawnedObjects = new List<GameObject>();
    private GameObject targetObject;
    private GameObject previousTarget;
    private float searchTimer = 0f;
    private ParticleSystem activeParticleTrail;
    private bool hintActive = false;

    // Referenzen zu den VR Controllern
    private Transform rightControllerTransform;
    private Transform leftControllerTransform;

    void Start()
    {
        SpawnObjects();
        searchTimer = 0f;

        // Finde die Controller Transforms
        FindControllerTransforms();
    }

    void FindControllerTransforms()
    {
        // Suche nach den OVR Controller Anchors
        OVRCameraRig cameraRig = FindObjectOfType<OVRCameraRig>();
        if (cameraRig != null)
        {
            rightControllerTransform = cameraRig.rightHandAnchor;
            leftControllerTransform = cameraRig.leftHandAnchor;
            Debug.Log("Controllers found!");
        }
        else
        {
            Debug.LogWarning("OVRCameraRig not found!");
        }
    }

    void Update()
    {
        if (spawnedObjects.Count == 0) return;

        targetObject = GetNearestObject();

        if (targetObject != previousTarget)
        {
            // Highlight vom alten Objekt entfernen
            if (previousTarget != null)
            {
                RemoveHighlight(previousTarget);
            }
            HighlightObject(targetObject);
            previousTarget = targetObject;
        }

        searchTimer += Time.deltaTime;

        if (!hintActive && searchTimer >= timeUntilHint)
        {
            ActivateHint();
        }

        if (hintActive && activeParticleTrail != null)
        {
            UpdateParticleTrail();
        }

        // VR Controller Raycast für rechten Controller
        if (OVRInput.GetDown(OVRInput.Button.One, rightController) ||
            OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, rightController) > 0.5f)
        {
            CheckControllerRaycast(rightControllerTransform);
        }

        // VR Controller Raycast für linken Controller
        if (OVRInput.GetDown(OVRInput.Button.One, leftController) ||
            OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, leftController) > 0.5f)
        {
            CheckControllerRaycast(leftControllerTransform);
        }

        // Fallback für Editor-Testing mit Maus
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                CheckIfSpawnedObject(hit.collider.gameObject);
            }
        }
    }

    void CheckControllerRaycast(Transform controllerTransform)
    {
        if (controllerTransform == null) return;

        Ray ray = new Ray(controllerTransform.position, controllerTransform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red, 0.5f);

        if (Physics.Raycast(ray, out hit, rayDistance, raycastLayerMask))
        {
            Debug.Log("Controller hit: " + hit.collider.gameObject.name);
            CheckIfSpawnedObject(hit.collider.gameObject);
        }
    }

    void CheckIfSpawnedObject(GameObject hitObject)
    {
        // Prüfe ob das getroffene Objekt in der Liste der gespawnten Objekte ist
        if (spawnedObjects.Contains(hitObject))
        {
            Debug.Log("Spawned object hit: " + hitObject.name);
            uiText.text = "Object touched!";
            OnObjectTouched(hitObject);
        }
    }

    void OnObjectTouched(GameObject touchedObject)
    {
        if (activeParticleTrail != null)
        {
            Destroy(activeParticleTrail.gameObject);
            activeParticleTrail = null;
        }
        hintActive = false;
        changeScene(targetSceneName);
    }

    GameObject GetNearestObject()
    {
        GameObject nearest = null;
        float nearestDist = float.MaxValue;

        foreach (GameObject obj in spawnedObjects)
        {
            float dist = Vector3.Distance(obj.transform.position, Player.transform.position);
            if (dist < nearestDist)
            {
                nearest = obj;
                nearestDist = dist;
            }
        }
        return nearest;
    }

    void SpawnObjects()
    {
        while (spawnableObjects.Count > 0)
        {
            int objIndex = Random.Range(0, spawnableObjects.Count);
            int posIndex = Random.Range(0, spawnPositions.Count);

            GameObject spawned = Instantiate(spawnableObjects[objIndex], spawnPositions[posIndex], Quaternion.identity);
            spawnedObjects.Add(spawned);

            spawnableObjects.RemoveAt(objIndex);
            spawnPositions.RemoveAt(posIndex);
        }
    }

    void CheckIfTargetObject(GameObject clickedObject)
    {
        if (clickedObject == targetObject)
        {
            uiText.text = "Object found!";
            OnTargetFound();
        }
        else
        {
            uiText.text = "Wrong object";
        }
    }

    void OnTargetFound()
    {
        if (activeParticleTrail != null)
        {
            Destroy(activeParticleTrail.gameObject);
            activeParticleTrail = null;
        }
        hintActive = false;
        changeScene("meditation_scene");
    }

    void HighlightObject(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer == null) return;

        Material mat = renderer.material;
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", Color.yellow * 2f);
    }

    void RemoveHighlight(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer == null) return;

        Material mat = renderer.material;
        mat.DisableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", Color.black);
    }

    void ActivateHint()
    {
        hintActive = true;

        if (particleTrailPrefab != null && Player != null && targetObject != null)
        {
            activeParticleTrail = Instantiate(particleTrailPrefab, Player.transform.position, Quaternion.identity);
            uiText.text = "Hint: Follow the particles!";
        }
    }

    void UpdateParticleTrail()
    {
        if (Player != null && targetObject != null)
        {
            activeParticleTrail.transform.position = Player.transform.position;

            Vector3 direction = (targetObject.transform.position - Player.transform.position).normalized;
            activeParticleTrail.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    void changeScene(String sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}