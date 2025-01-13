using UnityEngine;

public class khienmove : MonoBehaviour
{
    [SerializeField] private Transform target; // Đối tượng mà enemy sẽ theo
    [SerializeField] private Vector3 offset; // Khoảng cách giữa đối tượng và enemy

    private void Update()
    {
        if (target != null)
        {
            // Cập nhật vị trí của đối tượng để nó luôn theo sau đối tượng mục tiêu
            transform.position = target.position + offset;
        }
    }

    // Phương thức để gán mục tiêu
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}