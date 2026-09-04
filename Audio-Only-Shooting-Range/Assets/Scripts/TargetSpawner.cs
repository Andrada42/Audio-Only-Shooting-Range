using UnityEngine;
using System.Collections;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;

    // !!! Momentan manual: ne asiguram ca e in interiorul camerei
    [Header("Spawn Range")]     // fata de spawner
    public float minDistance = 7f;
    public float maxDistance = 15f;
    public float minHeight = 1f;
    public float maxHeight = 10f;

    [Header("Spawn Range")]
    public float respawnDelay = 1.5f;


    private GameObject currentTarget;
    private bool isWaitingToSpawn = false;
    

    void Start()
    {
        SpawnTarget();
    }

    void Update()
    {
        if (currentTarget == null && !isWaitingToSpawn)
        {
            StartCoroutine(SpawnRoutine());    // Corutina = functie ce poate fi pusa pe pauza si apoi reluata, fara sa blocheze jocul
        }
    }

    IEnumerator SpawnRoutine()  // IEnumerator => Corutina
    {
        isWaitingToSpawn = true;

        yield return new WaitForSeconds(respawnDelay);  // returneaza obiectul de tip WaitForSeconds => Unity stie cat sa astepte => apoi revine inapoi

        SpawnTarget();

        isWaitingToSpawn = false;
    }

    void SpawnTarget()
    {
        if (targetPrefab == null)
            return;

        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float randomDistance = Random.Range(minDistance, maxDistance);

        float x = transform.position.x + Mathf.Cos(randomAngle) * randomDistance;
        float z = transform.position.z + Mathf.Sin(randomAngle) * randomDistance;
    
        float y = Random.Range(minHeight, maxHeight);

        Vector3 spawnPosition = new Vector3(x, y, z);

        Debug.Log($"Spawnez la {spawnPosition.x}, {spawnPosition.y}, {spawnPosition.z}");
        currentTarget = Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
    }
}
