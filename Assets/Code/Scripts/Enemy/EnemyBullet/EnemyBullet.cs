using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("속도")]
    public float speed = 8f;
    [Header("생존 시간")]
    public float lifeTime = 2f;

    private Vector2 moveDir;

    void OnEnable()
    {
        // 발사 순간 플레이어 방향 고정
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
            moveDir = (player.transform.position - transform.position).normalized;
        else
            moveDir = transform.right;

        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // 생존 시간 후 풀로 반환
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 Bullet 피격");
            GameManager.Instance.playerController.TakeDamage(1);
            ReturnToPool();
        }

        if (!other.isTrigger && !other.CompareTag("Player") && !other.CompareTag("Enemy") && !other.CompareTag("Bullet"))
            ReturnToPool();
    }

    void ReturnToPool()
    {
        GameManager.Instance.poolManager.ReturnToPool(gameObject);
    }
    void OnDisable()
    {
        CancelInvoke();     // 풀에서 다시 꺼낼 때 중복 Invoke 방지
    }
}
