using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float normalSpeed = 12f;
    [SerializeField] private float detectionRange = 20f;
    [SerializeField] private float rotationSpeed = 720f; // Tốc độ xoay
    [SerializeField] private float dashDistance = 10f; // Khoảng cách tốc biến
    [SerializeField] private float dashCooldown = 5f; // Thời gian hồi chiêu sau khi tốc biến

    private NavMeshAgent agent;
    private Transform player;
    private bool canDash = true; // Biến kiểm tra xem có thể tốc biến hay không

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Kiểm tra xem người chơi có trong khoảng phát hiện không
            if (distanceToPlayer <= detectionRange)
            {
                MoveTowardsPlayer();
                RotateTowardsPlayer(); // Quay mặt về phía người chơi

                // Thực hiện tốc biến nếu có thể
                if (canDash && distanceToPlayer <= dashDistance)
                {
                    DashToPlayer();
                }
            }
            else
            {
                // Dừng lại nếu bên ngoài khoảng phát hiện
                agent.SetDestination(transform.position);
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        agent.SetDestination(player.position);
        agent.speed = normalSpeed;
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized; // Tính toán hướng đến người chơi
        if (direction != Vector3.zero) // Kiểm tra nếu hướng khác không
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Tính toán góc quay
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); // Xoay về hướng người chơi
        }
    }

    private void DashToPlayer()
    {
        // Tốc biến đến vị trí của người chơi
        transform.position = player.position; // Tốc biến đến vị trí người chơi

        // Đặt lại trạng thái tốc biến
        canDash = false;
        StartCoroutine(DashCooldownCoroutine());
    }

    private IEnumerator DashCooldownCoroutine()
    {
        yield return new WaitForSeconds(dashCooldown); // Chờ trong thời gian hồi chiêu
        canDash = true; // Cho phép tốc biến lại
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dashDistance); // Hiển thị khoảng cách tốc biến
    }
}