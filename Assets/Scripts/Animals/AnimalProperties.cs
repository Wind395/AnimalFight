using UnityEngine;
using Fusion;
using UnityEngine.UI;
using Unity.Cinemachine;
using System.Collections;
using TMPro;

public class AnimalProperties : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(OnNameChanged))]
    public string animalName { get; set; }
    public TextMeshProUGUI animalNameText;

    [Networked, OnChangedRender(nameof(OnHealthChanged))]
    public int health { get; set; }
    public Slider healthSlider;
    [Networked, OnChangedRender(nameof(OnPointChanged))] 
    public int point { get; set; } // Điểm của người chơi


    [Networked]
    public bool isDead { get; set; } // Đồng bộ trạng thái chết

    public CapsuleCollider capsuleCollider;
    private GameObject[] target;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    private void OnNameChanged()
    {
        // Cập nhật UI khi tên thay đổi
        if (animalNameText != null)
        {
            animalNameText.text = animalName;
            Debug.Log($"Animal name updated: {animalName}");
        }
        // Cập nhật bảng xếp hạng để phản ánh tên mới
        LeaderBoard.Instance?.UpdateLeaderboard();
    }

    private void OnHealthChanged()
    {
        healthSlider.value = health;
        Debug.Log($"Health: {healthSlider.value}");
    }

    public void OnPointChanged()
    {
        Debug.Log($"Point: {point}");
        LeaderBoard.Instance.UpdateLeaderboard(); // Cập nhật bảng xếp hạng khi điểm thay đổi
    }

    public void PointChanged(int point)
    {
        // Chỉ client có quyền mới có thể thay đổi điểm
        if (!Object.HasStateAuthority) return;

        health -= point;
        Debug.Log($"Point: {this.point}");
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_UpdatePoint(int point)
    {
        PointChanged(point);
    }



    public override void Spawned()
    {
        
        // Thiết lập giá trị ban đầu cho thanh máu
        healthSlider.value = health;

        if (!Object.HasStateAuthority)
        {
            gameObject.tag = "Target";
            healthSlider.gameObject.SetActive(false);
        }
        else
        {
            gameObject.tag = "Player";
            healthSlider.gameObject.SetActive(true);
        }
        point = 200; // Khởi tạo điểm
        LeaderBoard.Instance.RegisterPlayer(this); // Đăng ký người chơi vào bảng xếp hạng
    }

    void Update()
    {
        target = GameObject.FindGameObjectsWithTag("Target");
        //Debug.Log(LobbyManager.Instance.isGameStarted);
        if (target.Length <= 0 && LobbyManager.Instance.isGameStarted)
        {
            WinLoseUI.Instance.Win(); // Gọi hàm Win() từ WinLoseUI
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            WinLoseUI.Instance.Lose(); // Gọi hàm Win() từ WinLoseUI
        }

        //animalNameText.text = animalName;
    }

    public override void FixedUpdateNetwork()
    {
        animalNameText.text = animalName;
        if (LobbyManager.Instance.nameInput)
        {
            LobbyManager.Instance.nameInput = false; // Đặt lại biến nameInput sau khi đã sử dụng
        }
    }

    public override void Render()
    {
        // Cập nhật UI cho tất cả client
        if (animalNameText != null)
        {
            animalNameText.text = animalName;
        }
    }


    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_UpdateName(string newName, PlayerRef sender)
    {
        if (Object.HasStateAuthority && sender == Object.InputAuthority)
        {
            animalName = newName;
        }
    }



    public IEnumerator TakeDamage(int damage)
    {
        // Chỉ client có quyền mới có thể thay đổi máu
        if (!Object.HasStateAuthority) yield break;

        health -= damage;
        Debug.Log($"Health: {health}");

        if (health <= 0)
        {
            capsuleCollider.enabled = false; // Vô hiệu hóa collider khi chết
            isDead = true;
            WinLoseUI.Instance.Lose(); // Gọi hàm Lose() từ WinLoseUI
            //spectatorMode.ActivateSpectatorMode();
            Debug.Log("Player is dead. Switching to spectator mode.");
            //SwitchToSpectatorMode();
            yield return new WaitForSeconds(1f);
            Debug.Log("Player is dead.");
            Runner.Despawn(Object);
        }
    }

    // private void SwitchToSpectatorMode()
    // {
    //     // Tìm một đối tượng player khác để theo dõi (không bao gồm đối tượng này)
    //     GameObject[] players = GameObject.FindGameObjectsWithTag("Target");
    //     foreach (GameObject player in players)
    //     {
    //         if (player != this.gameObject)
    //         {
    //             spectatorMode.SetSpectatorMode(player.transform);
    //             Debug.Log("Spectator mode activated, following: " + player.name);
    //             return;
    //         }
    //     }
    //     Debug.Log("Không tìm thấy người chơi nào để quan sát.");
    // }

    



    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_TakeDamage(int damage)
    {
        StartCoroutine(TakeDamage(damage));
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        //base.Despawned(runner, hasState);
        LeaderBoard.Instance.UnregisterPlayer(this); // Hủy đăng ký người chơi khỏi bảng xếp hạng
    }
}