using System.Collections;
using UnityEngine;

public class EnemytKhien : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Tạo một enemy mới
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            // Chờ một khoảng thời gian trước khi sinh enemy tiếp theo
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}