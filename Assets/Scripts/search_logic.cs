using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class search_logic : MonoBehaviour
{

 // UI element to display text information to the player
public TextMeshProUGUI uiText;

// Reference to the player GameObject
[SerializeField] private GameObject Player;

// Manager handling the olfactory (scent) device
private OlfactoryDeviceManager OlfactoryDeviceManager;

// List of possible positions where objects can be spawned
public List<Vector3> spawnPositions;

// List of objects that can be spawned in the scene
public List<GameObject> spawnableObjects;

[Header("Hint Settings")]
// Time in seconds before a hint is shown to the player
public float timeUntilHint = 10f;

// Particle system prefab used to create a visual trail hint
public ParticleSystem particleTrailPrefab;

[Header("VR Controller Settings")]
// OVR input reference for the right controller
public OVRInput.Controller rightController = OVRInput.Controller.RTouch;

// OVR input reference for the left controller
public OVRInput.Controller leftController = OVRInput.Controller.LTouch;

// Maximum distance of the raycast from the controller
public float rayDistance = 10f;

// Distance threshold at which the ray becomes visible
public float rayShowDistance = 5f;

// Layer mask determining which objects the raycast can hit
public LayerMask raycastLayerMask = ~0;

[Header("Scene Settings")]
// Name of the target scene to load
public string targetSceneName = "meditation_scene";

[Header("Debug Settings")]
// UI element to display debug messages
public TextMeshProUGUI debugText;

// Toggle to enable or disable debug output
public bool showDebug = true;

// List of all currently spawned objects in the scene
public List<GameObject> spawnedObjects = new List<GameObject>();

// The current target object the player needs to find
private GameObject targetObject;

// The previously targeted object
private GameObject previousTarget;

// Tracks how long the player has been searching, used to check when to activate the particle trail
private float searchTimer = 0f;

// The currently active particle trail instance
private ParticleSystem activeParticleTrail;

// Whether the particle strail is currently active or not
private bool hintActive = false;

// Distance to the target in the previous frame (used to detect approach/retreat)
private float previousDistanceToTarget = float.MaxValue;

// Transform of the right controller for raycasting
private Transform rightControllerTransform;

// Transform of the left controller for raycasting
private Transform leftControllerTransform;

// Line renderer for visualizing the right controller ray
private LineRenderer rightRayLine;

// Line renderer for visualizing the left controller ray
private LineRenderer leftRayLine;

// Rolling list of recent debug messages
private List<string> debugMessages = new List<string>();

// Maximum number of debug messages to display at once
private int maxDebugMessages = 10;

// Timer to control debug message refresh rate
private float debugTimer = 0f;

// Toggle to show or hide the controller rays in the scene
public bool showRays = false;

    void Start()
    {
        SpawnObjects();
        searchTimer = 0f;
        FindControllerTransforms();

        AddDebugMessage($"Started! Spawned {spawnedObjects.Count} objects");
        UpdateDebugDisplay();
        OlfactoryDeviceManager = GetComponent<OlfactoryDeviceManager>();
    }

    void FindControllerTransforms()
    {
        OVRCameraRig cameraRig = FindObjectOfType<OVRCameraRig>();
        if (cameraRig != null)
        {
            rightControllerTransform = cameraRig.rightHandAnchor;
            leftControllerTransform = cameraRig.leftHandAnchor;
            rightRayLine = CreateRayLine(rightControllerTransform, "RightRay");
            leftRayLine = CreateRayLine(leftControllerTransform, "LeftRay");
            AddDebugMessage("Controllers found!");
        }
        else
        {
            AddDebugMessage("ERROR: OVRCameraRig not found!");
        }
    }

    void Update()
    {
        // Periodic debug output (every 2 seconds)
        debugTimer += Time.deltaTime;
        if (debugTimer >= 2f)
        {
            debugTimer = 0f;
            string rCtrl = rightControllerTransform != null ? "OK" : "NULL";
            string lCtrl = leftControllerTransform != null ? "OK" : "NULL";
            float rTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, rightController);
            float lTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, leftController);
            AddDebugMessage($"TICK | R:{rCtrl} L:{lCtrl} | RTrig:{rTrigger:F2} LTrig:{lTrigger:F2} | Objs:{spawnedObjects.Count}");
            UpdateDebugDisplay();
        }

        if (spawnedObjects.Count == 0)
        {
            AddDebugMessage("WARNING: No spawned objects!");
            UpdateDebugDisplay();
            return;
        }

        GameObject currentNearest = GetNearestObject();

        if (currentNearest != targetObject)
        {
            searchTimer = 0f;
            // destroy particles if target changed
            if (hintActive && activeParticleTrail != null)
            {
                Destroy(activeParticleTrail.gameObject);
                activeParticleTrail = null;
                hintActive = false;
                uiText.text = "";
            }

            targetObject = currentNearest;
            previousDistanceToTarget = targetObject != null
                ? Vector3.Distance(Player.transform.position, targetObject.transform.position)
                : float.MaxValue;
        }

        if (targetObject != previousTarget)
        {
            if (previousTarget != null)
            {
                RemoveHighlight(previousTarget);
            }
            previousTarget = targetObject;
        }

        // Check if player is moving in the direction of the target
        if (targetObject != null && Player != null)
        {
            float currentDistance = Vector3.Distance(Player.transform.position, targetObject.transform.position);

            if (currentDistance < previousDistanceToTarget - 0.05f)
            {
                searchTimer = 0f;

                if (hintActive && activeParticleTrail != null)
                {
                    Destroy(activeParticleTrail.gameObject);
                    activeParticleTrail = null;
                    hintActive = false;
                    uiText.text = "";
                }
            }

            previousDistanceToTarget = currentDistance;
        }

        searchTimer += Time.deltaTime;

        if (!hintActive && searchTimer >= timeUntilHint)
        {
            ActivateHint();
        }

        if (hintActive && activeParticleTrail != null)
        {
            UpdateParticleTrail();
            HighlightObject(targetObject);
        }

        // VR Controller Raycast for right controller
        if (OVRInput.GetDown(OVRInput.Button.One, rightController) ||
            OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, rightController) > 0.5f)
        {
            AddDebugMessage("RIGHT trigger pressed!");
            CheckControllerRaycast(rightControllerTransform, "RIGHT");
        }

        // VR Controller Raycast for left controller
        if (OVRInput.GetDown(OVRInput.Button.One, leftController) ||
            OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, leftController) > 0.5f)
        {
            AddDebugMessage("LEFT trigger pressed!");
            CheckControllerRaycast(leftControllerTransform, "LEFT");
        }

        UpdateRayVisuals();

        // Fallback for editor testing
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                AddDebugMessage($"Mouse hit: {hit.collider.gameObject.name}");
                CheckIfSpawnedObject(hit.collider.gameObject);
            }
        }

        UpdateDebugDisplay();
    }

    void CheckControllerRaycast(Transform controllerTransform, string controllerName)
    {
        if (controllerTransform == null)
        {
            AddDebugMessage($"{controllerName}: Transform NULL!");
            return;
        }

        Ray ray = new Ray(controllerTransform.position, controllerTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, raycastLayerMask))
        {
            AddDebugMessage($"{controllerName} HIT: {hit.collider.gameObject.name} (dist: {hit.distance:F2})");
            CheckIfSpawnedObject(hit.collider.gameObject);
        }
        else
        {
            AddDebugMessage($"{controllerName}: No hit (range: {rayDistance}m)");
        }
    }

    void CheckIfSpawnedObject(GameObject hitObject)
    {
        AddDebugMessage($"Checking: {hitObject.name}");

        if (spawnedObjects.Contains(hitObject))
        {
            AddDebugMessage($"SUCCESS! Scene change!");
            SceneManager.LoadScene("Gradient");
            uiText.text = "Object touched!";
            OnObjectTouched(hitObject);
        }
        else
        {
            AddDebugMessage($"NOT spawned object");
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
        SceneManager.LoadScene("Gradient");
    }

    // Debug-function
    void AddDebugMessage(string message)
    {
        if (!showDebug) return;

        debugMessages.Add($"[{Time.time:F1}] {message}");
        if (debugMessages.Count > maxDebugMessages)
        {
            debugMessages.RemoveAt(0);
        }

        Debug.Log(message); 
    }

    void UpdateDebugDisplay()
    {
        if (!showDebug || debugText == null) return;

        debugText.text = "=== DEBUG ===\n" + string.Join("\n", debugMessages);
    }

    LineRenderer CreateRayLine(Transform parent, string name)
    {
        GameObject rayObj = new GameObject(name);
        rayObj.transform.SetParent(parent, false);
        LineRenderer lr = rayObj.AddComponent<LineRenderer>();
        lr.startWidth = 0.005f;
        lr.endWidth = 0.005f;
        lr.positionCount = 2;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.white;
        lr.endColor = Color.white;
        lr.useWorldSpace = true;
        return lr;
    }

    void UpdateRayVisuals()
    {
        // Check if player is close enough to the target
        showRays = false;
        if (targetObject != null && Player != null)
        {
            float distanceToTarget = Vector3.Distance(Player.transform.position, targetObject.transform.position);
            showRays = distanceToTarget <= rayShowDistance;
        }

        UpdateSingleRay(rightControllerTransform, rightRayLine, showRays);
        UpdateSingleRay(leftControllerTransform, leftRayLine, showRays);
    }

    void UpdateSingleRay(Transform controller, LineRenderer line, bool showRay)
    {
        if (controller == null || line == null) return;

        // Disable rays if player is not close enough to the target
        line.enabled = showRay;
        if (!showRay) return;

        Vector3 start = controller.position;
        Vector3 end;

        RaycastHit hit;
        if (Physics.Raycast(start, controller.forward, out hit, rayDistance, raycastLayerMask))
        {
            end = hit.point;
            bool isTarget = hit.collider.gameObject == targetObject;
            Color rayColor = isTarget ? Color.green : Color.white;
            line.startColor = rayColor;
            line.endColor = rayColor;
        }
        else
        {
            end = start + controller.forward * rayDistance;
            line.startColor = Color.white;
            line.endColor = Color.white;
        }

        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }
}