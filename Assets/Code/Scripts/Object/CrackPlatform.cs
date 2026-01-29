using UnityEngine;
using tagName = Globals.TagName;

public class CrackPlatform : MonoBehaviour
{
    [Header("최대 내구도")]
    public int maxCount = 3;   // 최대 내구도
    public int count;          // 현재 내구도

    SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        count = maxCount;
        UpdateColor();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            count--;
            UpdateColor();
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tagName.throwingObj) || collision.gameObject.CompareTag(tagName.throwingEnemy))
        {
            count--;
            UpdateColor();
        }
    }

    void UpdateColor()
    {
        float ratio = (float)count / maxCount;

        if (ratio > 0.66f)          // 2/3 이상
            sprite.color = Color.white;   // 정상
        else if (ratio > 0.33f)     // 1/3 ~ 2/3
            sprite.color = Color.yellow;
        else if (ratio > 0f)        // 0 ~ 1/3
            sprite.color = Color.red;
        else                        // 파괴
            Destroy(gameObject);
    }
}
