using System.Collections;
using UnityEngine;

public class mauvano : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f; // Máu tối đa
    [SerializeField] private float currentHealth; // Máu hiện tại
    private BoxCollider boxCollider; // Collider của quái
    private AudioSource audioSource; // AudioSource để phát âm thanh
    [SerializeField] private AudioClip deathSound; // Tệp âm thanh khi chết
    [SerializeField] private GameObject explosionPrefab; // Prefab vụ nổ
    [SerializeField] private float explosionDamage = 50f; // Sát thương từ vụ nổ
    [SerializeField] private float explosionRadius = 5f; // Bán kính vụ nổ
    [SerializeField] private float detectionRadius = 10f; // Bán kính phát hiện người chơi
    [SerializeField] private float explosionDelay = 2f; // Thời gian chờ trước khi phát nổ

    private Transform player; // Tham chiếu đến người chơi
    private bool hasExploded = false; // Kiểm tra xem đã phát nổ hay chưa

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>(); // Lấy BoxCollider
        audioSource = GetComponent<AudioSource>(); // Lấy AudioSource
        currentHealth = maxHealth;

        // Tìm đối tượng người chơi
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Debug thông tin
        Debug.Log("Current Health: " + currentHealth);
    }

    private void Update()
    {
        if (player != null && !hasExploded)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Kiểm tra nếu người chơi còn trong phạm vi phát nổ
            if (distanceToPlayer <= detectionRadius)
            {
                StartCoroutine(PrepareExplosion()); // Bắt đầu coroutine chuẩn bị phát nổ
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Kiểm tra nếu va chạm với đối tượng có layer "Bullet"
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet")) // Kiểm tra layer "Bullet"
        {
            TakeDamage(110f); // Nhận sát thương 110
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0); // Đảm bảo máu không âm

        Debug.Log("Damage Taken: " + damage + ", Current Health: " + currentHealth);

        if (currentHealth <= 0f)
        {
            HandleDeath();
        }
    }

    private IEnumerator PrepareExplosion()
    {
        if (!hasExploded)
        {
            // Đợi trong khoảng thời gian chỉ định trước khi phát nổ
            yield return new WaitForSeconds(explosionDelay);
            HandleDeath(); // Gọi hàm phát nổ
        }
    }

    private void HandleDeath()
    {
        if (hasExploded) return; // Nếu đã phát nổ thì không làm gì nữa

        hasExploded = true; // Đánh dấu là đã phát nổ
        boxCollider.enabled = false; // Vô hiệu hóa collider

        // Phát âm thanh khi chết
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Tạo vụ nổ
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            DealExplosionDamage(); // Gây sát thương cho người chơi
        }

        // Phá hủy đối tượng sau một khoảng thời gian để âm thanh có thể phát
        Destroy(gameObject, 1f); // Thay đổi thời gian theo nhu cầu (1 giây trong ví dụ này)

        // Debug thông tin
        Debug.Log("Enemy has died and has been destroyed.");
    }

    private void DealExplosionDamage()
    {
        // Tìm tất cả các đối tượng trong bán kính vụ nổ
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            // Kiểm tra xem đối tượng có tag "Player" không
            if (hitCollider.CompareTag("Player"))
            {
                // Gọi phương thức giảm máu cho người chơi
                PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(explosionDamage, 0f); // Gây sát thương cho người chơi với penetration = 0
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Vẽ bán kính phát hiện
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Vẽ bán kính vụ nổ
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}