using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    public PlayableDirector director;

    bool hasPlayed = false; // 중복 체크

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (hasPlayed) return;  // 이미 재생된 경우 무시

            hasPlayed = true;
            director.Play();
        }
    }
}
