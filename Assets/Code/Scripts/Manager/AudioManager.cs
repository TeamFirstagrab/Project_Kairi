using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("배경음악")]
    public AudioSource bgmSource;

    [Header("효과음")]
    public AudioSource sfxSource;

    [Header("플레이어 효과음")]
    public AudioClip playerJump;

    [Header("갈고리 효과음")]
    public AudioClip hookAttach;
    public AudioClip hookDraft;
    public AudioClip hookShoot;
    public AudioClip hookThrowEnemy;

    // 배경음 재생
    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmSource == null || clip == null) return;

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    // 배경음 정지
    public void StopBGM()
    {
        if (bgmSource != null) bgmSource.Stop();
    }

    // 효과음 재생
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (sfxSource != null && clip != null) sfxSource.PlayOneShot(clip, volume);
    }
    public void StopSFX()
    {
        if (sfxSource == null) return;

        sfxSource.Stop();
    }
    // 편의 함수

    // 플레이어
    public void PlayJumpSound(float volume = 1f) => PlaySFX(playerJump, volume);

    // 갈고리
    public void HookAttachSound(float volume = 1f) => PlaySFX(hookAttach, volume);
    public void HookDraftSound(float volume = 1f) => PlaySFX(hookDraft, volume);
    public void HookShootSound(float volume = 1f) => PlaySFX(hookShoot, volume);
    public void HookThrowEnemySound(float volume = 1f) => PlaySFX(hookThrowEnemy, volume);
}