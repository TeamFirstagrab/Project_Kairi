using UnityEngine;

public class PlayerHPBar : MonoBehaviour
{
    [Header("HP바 프리팹")]
    public GameObject hpBarPrefab;      // 프리팹 에셋
    private GameObject hpBarInstance;   // 씬에 생성된 오브젝트
    [Header("따라다닐 오프셋")]
    public Vector3 offset = new Vector3(-1f, 1f, 0f);
    [Header("작을수록 더 빨리 따라옴")]
    public float smoothTime = 0.15f;
    Vector3 velocity;                   // SmoothDamp 필수 변수
    SpriteRenderer sprite;

    void Awake()
    {
        hpBarInstance = Instantiate(hpBarPrefab);
        hpBarInstance.transform.position = transform.position + offset;
        sprite = hpBarInstance.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (hpBarInstance == null) return;

        Vector3 targetPos = transform.position + offset;

        hpBarInstance.transform.position = Vector3.SmoothDamp(
                hpBarInstance.transform.position,
                targetPos,
                ref velocity,
                smoothTime
            );

        float curHP = GameManager.Instance.playerStatsRuntime.currentHP;

        switch (curHP)
        {
            case 5: sprite.color = Color.cyan; break;
            case 4: sprite.color = Color.green; break;
            case 3: sprite.color = Color.yellow; break;
            case 2: sprite.color = new Color(1f, 0.5f, 0f); break;
            case 1: sprite.color = Color.red; break;
            case 0: sprite.color = Color.black; break;
        }
    }
    void OnDestroy()
    {
        if (hpBarInstance != null)      // 플레이어 죽으면 체력바도 제거
            Destroy(hpBarInstance);
    }
}
