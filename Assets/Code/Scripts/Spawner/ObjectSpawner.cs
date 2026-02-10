using System.Collections;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("스폰 정보")]
    public string objectPoolName;
    public float respawnDelay = 3f;

    ObjectController currentObject;
    bool isSpawning = false;

    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        if (isSpawning) return;

        GameObject obj = GameManager.Instance.poolManager
            .SpawnFromPool(objectPoolName, transform.position, Quaternion.identity);

        if (obj == null) return;

        currentObject = obj.GetComponent<ObjectController>();
        currentObject.Init(this);   // 스포너 전달

        isSpawning = true;
    }

    public void OnObjectDestroyed(ObjectController obj)
    {
        if (obj != currentObject) return;

        currentObject = null;
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        isSpawning = false;
        Spawn();
    }
}
