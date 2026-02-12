using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 1.0f;

    void Start()
    {
        // spawnInterval•b‚²‚Æ‚ÉSpawnEnemyŠÖ”‚ğŒÄ‚Ô
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    void SpawnEnemy()
    {
        // XÀ•W‚ğƒ‰ƒ“ƒ_ƒ€‚ÉŒˆ’è
        float randomX = Random.Range(-8f, 8f);
        Vector3 spawnPos = new Vector3(randomX, 6f, 0f);

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}