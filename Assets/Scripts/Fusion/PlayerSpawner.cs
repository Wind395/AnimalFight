using UnityEngine;
using Fusion;
using TMPro;
using UnityEngine.UI;
using System.Collections;


public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{

    public GameObject PlayerPrefab;
    public Button startGameButton;
    public GameObject lobbyManager;
    private LobbyManager lobbyManagers;



    void Start()
    {
        lobbyManagers = lobbyManager.GetComponent<LobbyManager>();
    }

    void Update()
    {
        if (Runner.LocalPlayer != lobbyManagers.roomOwner)
        {
            startGameButton.gameObject.SetActive(false);
            //Debug.Log("Not Room Owner, Button Disabled");
            return;
        }
        else
        {
            startGameButton.gameObject.SetActive(true);
            //Debug.Log("Room Owner, Button Enabled");
            return;
        }
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity, player);
        }

        lobbyManagers.RPC_AddPlayer(player);
        //NetworkObject playerObject = Runner.Spawn(PlayerPrefab, Vector3.zero, Quaternion.identity, Runner.LocalPlayer);
        //playerObject.gameObject.SetActive(false); // Disable the player object until the game starts
        Debug.Log("Player Joined: " + player);
        startGameButton.onClick.AddListener(() => OnStartGame());
    }

    public void OnStartGame()
    {
        if (lobbyManagers.playerCount >= lobbyManagers.minPlayers)
        {
            Debug.Log("Start Game");
            // Gọi hàm DelaySpawn để delay spawn player
            //StartCoroutine(DelaySpawn(player));
            RPC_StartGame();
        }
    }

    public void RPC_StartGame()
    {
        StartCoroutine(DelaySpawn());
    }
    
    public IEnumerator DelaySpawn()
    {
        lobbyManagers.RPC_UpdateSignalText();
        yield return new WaitForSeconds(3f); // Delay 3 giây trước khi spawn
        //gameObject.transform.GetChild(0).gameObject.SetActive(false);
        lobbyManagers.RPC_HideRoomUI();
    }
}
