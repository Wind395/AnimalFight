using UnityEngine;
using Fusion;
using System.Collections;

public class WeaponSpawn : NetworkBehaviour
{
    [Header("Weapon Spawn Settings")]
    public GameObject weaponPrefab;       // Prefab của vũ khí (đã đăng ký trong Network Prefabs)
    public int maxWeaponCount = 5;        // Số lượng vũ khí tối đa cần spawn
    public float spawnDelayMin = 0.5f;    // Delay spawn tối thiểu
    public float spawnDelayMax = 1.0f;    // Delay spawn tối đa
    public float spawnRadius = 5.0f;      // Bán kính spawn vũ khí

    private int currentWeaponCount = 0;  // Số lượng vũ khí đã spawn
    private float nextSpawnTime = 0f;    // Thời gian kế tiếp để spawn

    public float duration = 15f; // Thời gian chờ để spawn vũ khí
    public NetworkBool canSpawn = false; // Biến kiểm tra xem có thể spawn hay không

    //private LobbyManager lobbyManager;

    // void Start()
    // {
    //     // Tìm LobbyManager trong scene (đảm bảo LobbyManager có thuộc tính isGameStarted)
    //     lobbyManager = FindObjectOfType<LobbyManager>();
    // }

    public override void FixedUpdateNetwork()
    {
        
    }

    public override void Render()
    {
        //Debug.Log("LobbyManager.Instance.isGameStarted" + LobbyManager.Instance.isGameStarted);
        if (LobbyManager.Instance.isGameStarted)
        {
            StartCoroutine(Wait()); // Bắt đầu coroutine Wait() để spawn vũ khí
        }

        // if (Input.GetKeyDown(KeyCode.C))
        // {
        //     ObjectPooling.Instance.GetObject();
        // }
    }

    public void SpawnWeapon()
    {
        currentWeaponCount++;

        // Tính toán vị trí spawn: chọn hướng ngẫu nhiên trên mặt phẳng XZ
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection.y = 0; // Giới hạn spawn trên mặt phẳng ngang
        Vector3 spawnPosition = transform.position + randomDirection.normalized * spawnRadius;

        // Sử dụng Runner.Spawn để spawn vũ khí
        //Runner.Spawn(weaponPrefab, spawnPosition, Quaternion.identity);
        GameObject weapon = ObjectPooling.Instance.GetObject();
        weapon.transform.position = spawnPosition;
        Debug.Log("Spawned weapon at: " + spawnPosition);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(duration);
        canSpawn = true; // Cho phép spawn vũ khí sau thời gian chờ
        // Kiểm tra delay và số lượng vũ khí hiện tại
        if (canSpawn && currentWeaponCount < maxWeaponCount)
        {
            //Debug.Log("Can spawn weapon");
            SpawnWeapon(); // Gọi hàm spawn vũ khí
            canSpawn = false; // Đặt lại biến canSpawn để không spawn liên tục
            //yield return new WaitForSeconds(Random.Range(spawnDelayMin, spawnDelayMax)); // Thời gian delay trước khi spawn tiếp theo
        }
    }
}
