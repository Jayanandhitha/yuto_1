using UnityEngine;

public class SoulSpawner : MonoBehaviour
{
    public GameObject soulPrefab;

    public float spawnInterval = 8f;
    public float spawnRadius = 20f;

    float timer;

    public bool canSpawn = true; // 👈 NEW

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        if (!canSpawn) return; // 👈 STOP spawning

        if (soulPrefab == null)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnSoul();
            timer = 0f;
        }
    }

    void SpawnSoul()
    {
        Vector2 randomPosition = Random.insideUnitCircle * spawnRadius;

        Vector3 spawnPos = transform.position + new Vector3(
            randomPosition.x,
            randomPosition.y,
            0
        );

        Instantiate(soulPrefab, spawnPos, Quaternion.identity);
    }
}