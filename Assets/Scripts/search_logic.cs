using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class search_logic : MonoBehaviour
{

    public TextMeshProUGUI uiText;
    [SerializeField] private GameObject Player;
    private OlfactoryDeviceManager OlfactoryDeviceManager;

    public List<Vector3> spawnPositions;
    public List<GameObject> spawnableObjects;

    [Header("Hint Settings")]
    public float timeUntilHint = 10f;
    public ParticleSystem particleTrailPrefab;

    [Header("VR Controller Settings")]
    public OVRInput.Controller rightController = OVRInput.Controller.RTouch;
    public OVRInput.Controller leftController = OVRInput.Controller.LTouch;
    public float rayDistance = 10f;
    public float rayShowDistance = 5f; 
    public LayerMask raycastLayerMask = ~0;

    [Header("Scene Settings")]
    public string targetSceneName = "meditation_scene";

    [Header("Debug Settings")]
    public TextMeshProUGUI debugText; // Zus�tzliches Text-Feld f�r Debug-Info
    public bool showDebug = true;

    public List<GameObject> spawnedObjects = new List<GameObject>();
    private GameObject targetObject;
    private GameObject previousTarget;
    private float searchTimer = 0f;
    private ParticleSystem activeParticleTrail;
    private bool hintActive = false;

    private Transform rightControllerTransform;
    private Transform leftControllerTransform;
    private LineRenderer rightRayLine;
    private LineRenderer leftRayLine;

    private List<string> debugMessages = new List<string>();
    private int maxDebugMessages = 10;
    private float debugTimer = 0f;

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
        // Periodischer Debug-Output alle 2 Sekunden
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

            if (hintActive && activeParticleTrail != null)
            {
                Destroy(activeParticleTrail.gameObject);
                activeParticleTrail = null;
                hintActive = false;
                uiText.text = "";
            }

            targetObject = currentNearest;
        }

        if (targetObject != previousTarget)
        {
            if (previousTarget != null)
            {
                RemoveHighlight(previousTarget);
            }
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
            HighlightObject(targetObject);
        }

        // VR Controller Raycast f�r rechten Controller
        if (OVRInput.GetDown(OVRInput.Button.One, rightController) ||
            OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, rightController) > 0.5f)
        {
            AddDebugMessage("RIGHT trigger pressed!");
            CheckControllerRaycast(rightControllerTransform, "RIGHT");
        }

        // VR Controller Raycast f�r linken Controller
        if (OVRInput.GetDown(OVRInput.Button.One, leftController) ||
            OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, leftController) > 0.5f)
        {
            AddDebugMessage("LEFT trigger pressed!");
            CheckControllerRaycast(leftControllerTransform, "LEFT");
        }

        UpdateRayVisuals();

        // Fallback f�r Editor-Testing
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

    // Debug-Funktionen
    void AddDebugMessage(string message)
    {
        if (!showDebug) return;

        debugMessages.Add($"[{Time.time:F1}] {message}");
        if (debugMessages.Count > maxDebugMessages)
        {
            debugMessages.RemoveAt(0);
        }

        Debug.Log(message); // Auch in normale Console
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
        // Prüfe ob Spieler nah genug am Target ist
        bool showRays = false;
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

        // Ray ausblenden wenn Spieler nicht nah genug am Target ist
        line.enabled = showRay;
        if (!showRay) return;

        Vector3 start = controller.position;
        Vector3 end;

        RaycastHit hit;
        if (Physics.Raycast(start, controller.forward, out hit, rayDistance, raycastLayerMask))
        {
            end = hit.point;
            line.startColor = Color.green;
            line.endColor = Color.green;
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