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

    [Header("Trigger Settings")]
    public string triggerSceneName = "meditation_scene";

    public List<GameObject> spawnedObjects = new List<GameObject>();
    private GameObject targetObject;
    private GameObject previousTarget;
    private float searchTimer = 0f;
    private ParticleSystem activeParticleTrail;
    private bool hintActive = false;

    void Start()
    {
        SpawnObjects();
        searchTimer = 0f;

        // Füge TriggerHandler-Komponente zu gespawnten Objekten hinzu
        foreach (GameObject obj in spawnedObjects)
        {
            TriggerHandler handler = obj.AddComponent<TriggerHandler>();
            handler.Initialize(this);
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

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                CheckIfTargetObject(hit.collider.gameObject);
            }
        }
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

    // Diese Methode wird vom TriggerHandler aufgerufen
    public void OnObjectTriggered(GameObject triggeredObject)
    {
        uiText.text = "Object touched!";
        changeScene(triggerSceneName);
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

// Hilfs-Komponente für Trigger-Events
public class TriggerHandler : MonoBehaviour
{
    private search_logic searchLogic;

    public void Initialize(search_logic logic)
    {
        searchLogic = logic;
    }

    void OnTriggerEnter(Collider other)
    {
        // Prüfe ob der Player-Controller den Trigger berührt
        if (other.CompareTag("Player") || other.gameObject.name.Contains("Player"))
        {
            if (searchLogic != null)
            {
                searchLogic.OnObjectTriggered(gameObject);
            }
        }
    }
}