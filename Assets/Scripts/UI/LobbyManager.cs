using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using TMPro;
using UnityEngine;

public class LobbyManager : NetworkBehaviour
{

    public static LobbyManager Instance;

    [Networked]
    public PlayerRef roomOwner { get; set; }

    [Networked]
    public int playerCount { get; set; }
    public int currentPlayer = 0;
    public TextMeshProUGUI signalText;
    public int minPlayers = 1;
    public NetworkBool isGameStarted = false;

    public NetworkBool isGameEnded = false; // Trạng thái trò chơi kết thúc
    public bool nameInput = false; // Trạng thái nhập tên người chơi

    [SerializeField] private GameObject endGameLeaderboardPanel; // Panel hiển thị bảng xếp hạng cuối trận


    public FusionBootstrap fusionBootstrap;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        isGameEnded = false; // Khởi tạo trạng thái trò chơi chưa kết thúc
    }
    
    void Awake()
    {
        isGameStarted = false; // Khởi tạo trạng thái trò chơi chưa bắt đầu
    }

    // public override void Spawned()
    // {
    //     if (Object.HasStateAuthority)
    //     {
    //         playerCount = 0; // Sẽ được cập nhật khi người chơi tham gia
    //     }
    // }

    // Update is called once per frame
    void Update()
    {
        // if (isGameEnded)
        // {
        //     OutOfTime();
        // }
    }

    public override void FixedUpdateNetwork()
    {
        
    }

    public void OutOfTime()
    {
        if (!Object.HasStateAuthority) return;

        // Gọi RPC để hiển thị bảng xếp hạng trên tất cả client
        RPC_ShowEndGameLeaderboard();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ShowEndGameLeaderboard()
    {
        // Tìm tất cả người chơi còn sống
        AnimalProperties[] animalProperties = FindObjectsOfType<AnimalProperties>();
        if (animalProperties.Length >= 2)
        {
            // Sắp xếp người chơi theo điểm
            var sortedPlayers = animalProperties.OrderByDescending(p => p.point).ToList();
            AnimalProperties topPlayer = sortedPlayers[0]; // Top 1

            // Hiển thị bảng xếp hạng cuối trận
            if (endGameLeaderboardPanel != null)
            {
                endGameLeaderboardPanel.SetActive(true);
            }
            LeaderBoard.Instance?.ShowEndGameLeaderboard(animalProperties);

            // Hiển thị UI thắng/thua cho từng client
            foreach (var player in animalProperties)
            {
                if (player == topPlayer)
                {
                    // Top 1 hiển thị winUI
                    if (player.Object.InputAuthority == Runner.LocalPlayer)
                    {
                        WinLoseUI.Instance.Win();
                    }
                }
                else
                {
                    // Các người chơi khác hiển thị loseUI
                    if (player.Object.InputAuthority == Runner.LocalPlayer)
                    {
                        WinLoseUI.Instance.Lose();
                    }
                }
            }
        }
        else
        {
            Debug.Log("Not enough players to show leaderboard (less than 2).");
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_UpdateSignalText()
    {
        signalText.text = "Waiting for 3 seconds... ";
        Debug.Log("Signal Text Updated: " + signalText.text);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_AddPlayer(PlayerRef player)
    {
        if (roomOwner == default)
        {
            roomOwner = player;
            Debug.Log("Room Owner Assigned: " + player);
        }

        playerCount++;
        Debug.Log("Player Added");
    }


    //[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_HideRoomUI()
    {
        // Giả sử UI Room nằm ở con thứ nhất của đối tượng này
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        isGameStarted = true;
        GameManager.Instance.canMove = true;
        Debug.Log(GameManager.Instance.canMove);
    }

    public void JoinSharedRoom(GameObject joinRoomPanel, string roomName)
    {
        if (string.IsNullOrEmpty(roomName))
        {
            Debug.LogError("Room name cannot be empty.");
            return;
        }

        if (fusionBootstrap == null)
        {
            Debug.LogError("FusionBootstrap is not assigned.");
            return;
        }

        // Join the room using FusionBootstrap
        fusionBootstrap.DefaultRoomName = roomName;

        fusionBootstrap.StartSharedClient();
        joinRoomPanel.SetActive(false);
    }

    public void PlayerNameChange(GameObject playerNamePanel, string playerName)
    {
        if (string.IsNullOrEmpty(playerName) && !nameInput)
        {
            Debug.LogError("Player name cannot be empty.");
            return;
        }

        nameInput = true;
        playerNamePanel.SetActive(false);
    }
}
