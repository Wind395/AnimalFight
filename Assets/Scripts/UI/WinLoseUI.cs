using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;

public class WinLoseUI : NetworkBehaviour
{

    public static WinLoseUI Instance;

    public GameObject winUI;
    public GameObject loseUI;
    public GameObject playerPrefab;

    void Awake()
    {
        Instance = this;
        winUI.SetActive(false);
        loseUI.SetActive(false);
    }


    public void Win()
    {
        //Debug.Log("Win");
        winUI.SetActive(true);
        LobbyManager.Instance.isGameEnded = true;
    }


    public void Lose()
    {
        Debug.Log("Lose");
        loseUI.SetActive(true);
        //gameObject.SetActive(false);
    }

    public void SpectatorModeActive()
    {
        Debug.Log("Spectator Mode");
        SpectatorMode.Instance.IsSpectatorModeActive = true;
        loseUI.SetActive(false);
    }

    // public void Revive()
    // {
    //     Debug.Log("Revive");
    //     // if (player == Runner.LocalPlayer)
    //     // {
    //     //     Runner.Spawn(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity, player);   
    //     // }
    // }

    public void Exit()
    {
        Debug.Log("Exit");
        SceneManager.LoadScene("InGame");
    }
}
