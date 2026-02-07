using UnityEngine;

public class PlayerHPBar : MonoBehaviour
{
    public GameObject hpBarObject;
    SpriteRenderer sprite;

    public Vector3 offset = new Vector3(-1f, 1f, 0f);

    public float smoothTime = 0.15f;   // 작을수록 더 빨리 따라옴
    Vector3 velocity = Vector3.zero;  // SmoothDamp 필수 변수

    void Awake()
    {
        sprite = hpBarObject.GetComponent<SpriteRenderer>();
        hpBarObject.transform.position = transform.position + offset;
    }
    void FixedUpdate()
    {
        if (hpBarObject == null) return;

        Vector3 targetPos = transform.position + offset;

        hpBarObject.transform.position =
            Vector3.SmoothDamp(
                hpBarObject.transform.position,
                targetPos,
                ref velocity,
                smoothTime,
                Mathf.Infinity,
                Time.fixedDeltaTime
            );
    }

    void LateUpdate()
    {
        if (hpBarObject == null) return;

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
}
