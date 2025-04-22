using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public TMP_InputField roomNameInputField;
    [SerializeField] private GameObject roomUI;
    public TMP_InputField playerNameInputField;
    [SerializeField] private GameObject playerNameUI;
    public LobbyManager lobbyManager;

    public void OnJoinButtonClick()
    {
        string roomName = roomNameInputField.text;
        if (lobbyManager != null)
        {
            lobbyManager.JoinSharedRoom(roomUI, roomName);
        }
        else
        {
            Debug.LogError("LobbyManager is not assigned in UIManager.");
        }
    }

    public void OnPlayerNameChange()
    {
        string playerName = playerNameInputField.text;
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogError("Player name cannot be empty.");
            return;
        }

        if (lobbyManager != null)
        {
            lobbyManager.PlayerNameChange(playerNameUI, playerName);

            var animalProperties = GetLocalAnimalProperties();
            // Gọi RPC để cập nhật tên trên server
            // var animalProperties = FindAnyObjectByType<AnimalProperties>();
            if (animalProperties != null && animalProperties.Object.HasInputAuthority)
            {
                Debug.Log("Updating name");
                Debug.Log("Updating player name: " + playerName);
                animalProperties.Rpc_UpdateName(playerName, animalProperties.Runner.LocalPlayer);
            }
        }
        else
        {
            Debug.LogError("LobbyManager is not assigned in UIManager.");
        }
    }

    private AnimalProperties GetLocalAnimalProperties()
    {
        // Tìm tất cả các AnimalProperties trong scene
        var allAnimalProperties = FindObjectsOfType<AnimalProperties>();
        foreach (var animal in allAnimalProperties)
        {
            if (animal.Object.InputAuthority == animal.Runner.LocalPlayer)
            {
                return animal; // Trả về instance của người chơi cục bộ
            }
        }
        return null; // Trả về null nếu không tìm thấy
    }
}
