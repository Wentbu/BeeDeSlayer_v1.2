using System.Collections;
using UnityEngine;

public class Khien : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f; // Máu tối đa
    [SerializeField] private float currentHealth; // Máu hiện tại
    private SphereCollider sphereCollider; // Collider của quái
    private AudioSource audioSource; // AudioSource để phát âm thanh
    [SerializeField] private AudioClip deathSound; // Tệp âm thanh khi chết

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        audioSource = GetComponent<AudioSource>(); // Lấy AudioSource
        currentHealth = maxHealth;

        // Debug thông tin
        Debug.Log("Current Health: " + currentHealth);
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

    private void HandleDeath()
    {
        sphereCollider.enabled = false; // Vô hiệu hóa collider

        // Phát âm thanh khi chết
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Phá hủy đối tượng sau một khoảng thời gian để âm thanh có thể phát
        Destroy(gameObject, 1f); // Thay đổi thời gian theo nhu cầu (1 giây trong ví dụ này)

        // Debug thông tin
        Debug.Log("Enemy has died and has been destroyed.");
    }
}