using UnityEngine;
using System.Collections;

public class ShieldSpawner : MonoBehaviour
{
    [SerializeField] private GameObject shieldPrefab; // Prefab của khiên
    [SerializeField] private GameObject targetObject; // Đối tượng mà khiên sẽ theo
    [SerializeField] private float spawnInterval = 5f; // Thời gian giữa các lần spawn

    private void Start()
    {
        // Bắt đầu coroutine để spawn khiên định kỳ
        StartCoroutine(SpawnShield());
    }

    private IEnumerator SpawnShield()
    {
        while (true)
        {
            SpawnShieldInstance(); // Gọi phương thức spawn khiên
            yield return new WaitForSeconds(spawnInterval); // Chờ trước khi spawn lần tiếp theo
        }
    }

    private void SpawnShieldInstance()
    {
        if (shieldPrefab != null)
        {
            // Tạo một bản sao của prefab khiên tại vị trí của spawner
            GameObject shield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
            Debug.Log("Shield spawned at " + transform.position);

            // Gán đối tượng di chuyển theo khiên
            ShieldMovement shieldMovement = shield.AddComponent<ShieldMovement>();
            shieldMovement.target = targetObject; // Gán đối tượng mà khiên sẽ theo
        }
        else
        {
            Debug.LogWarning("Shield prefab is not assigned.");
        }
    }
}

public class ShieldMovement : MonoBehaviour
{
    public GameObject target; // Đối tượng mà khiên sẽ theo
    public float speed = 10f; // Tốc độ di chuyển của khiên
    public float distanceFromTarget = 1.5f; // Khoảng cách giữ giữa khiên và đối tượng

    private void Update()
    {
        if (target != null)
        {
            // Tính toán vị trí mới
            Vector3 targetPosition = target.transform.position - (target.transform.forward * distanceFromTarget);
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}
