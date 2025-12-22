using UnityEngine;

public class GameManager : MonoSingleton<GameManager> // 싱글톤 사용
{
    [Header("Manager 관련 코드")]
    public AudioManager audioManager;
    public PoolManager poolManager;

    [Header("카메라 관련 코드")]
    public CameraShake cameraShake;

    [Header("컨트롤러")]
    public PlayerController playerController;

    [Header("스탯")]
    public PlayerStats playerStats;
    public EnemyStats enemyStats;

    [Header("게임 실행 중 플레이어 스텟값 수정")]
    public PlayerStatsRuntime playerStatsRuntime;
    public EnemyStatsRuntime enemyStatsRuntime;

    protected new void Awake()
    {
        QualitySettings.vSyncCount = 0; // VSync 비활성화 (모니터 주사율 영향 제거)

        Application.targetFrameRate = 120; // 프레임 120 제한
        if (Instance != null && Instance != this) // 중복 GameManager 방지
        {
            Destroy(gameObject);
            return;
        }

        base.Awake(); // MonoSingleton의 Awake 호출

        // 플레이어 초기화
        playerStatsRuntime = new PlayerStatsRuntime(playerStats);   // 스탯 값 복제
    }
}
