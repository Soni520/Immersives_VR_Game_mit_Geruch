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
    public Transform player;

    public List<Vector3> spawnPositions;
    public List<GameObject> spawnableObjects;

    [Header("Hint Settings")]
    public float timeUntilHint = 10f;
    public ParticleSystem particleTrailPrefab;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private GameObject targetObject;
    private float searchTimer = 0f;
    private ParticleSystem activeParticleTrail;
    private bool hintActive = false;

    // Start is called before the first frame update
    void Start()
    {
        SpawnObjects();

        int randomObjectIndex = Random.Range(0, spawnedObjects.Count);
        targetObject = spawnedObjects[randomObjectIndex];

        HighlightObject(targetObject);
        searchTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObject != null)
        {
            searchTimer += Time.deltaTime;

            if (!hintActive && searchTimer >= timeUntilHint)
            {
                ActivateHint();
            }

            if (hintActive && activeParticleTrail != null)
            {
                UpdateParticleTrail();
            }
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
            changeScene("meditation_scene");
        }
        hintActive = false;
    }

    void HighlightObject(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError($"GameObject {obj.name} does not have a Renderer component!");
            return;
        }

        Material mat = renderer.material;
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", Color.yellow * 2f);
    }

    void ActivateHint()
    {
        hintActive = true;

        if (particleTrailPrefab != null && player != null && targetObject != null)
        {
            activeParticleTrail = Instantiate(particleTrailPrefab, player.position, Quaternion.identity);
            uiText.text = "Hint: Follow the particles!";
        }
        else
        {
            Debug.LogWarning("Cannot activate hint: Missing particleTrailPrefab, player, or targetObject reference!");
        }
    }

    void UpdateParticleTrail()
    {
        if (player != null && targetObject != null)
        {
            activeParticleTrail.transform.position = player.position;

            Vector3 direction = (targetObject.transform.position - player.position).normalized;
            activeParticleTrail.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    void changeScene(String sceneName)
    {
        SceneManager.LoadScene(sceneName);
        
    }
}
