using UnityEngine;

public class SoulSpawner : MonoBehaviour
{
    public GameObject soulPrefab;

    public float spawnInterval = 5f;
    public float spawnRadius = 15f;

    private float timer;

    void Update()
    {
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

        Vector3 spawnPos = new Vector3(
            randomPosition.x,
            randomPosition.y,
            0
        );

        Instantiate(soulPrefab, spawnPos, Quaternion.identity);
    }
}