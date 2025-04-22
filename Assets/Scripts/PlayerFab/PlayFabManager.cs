using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class PlayFabManager : MonoBehaviour
{

    private void Start()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    public void SavePlayerData(string key, string value)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { key, value }
            }
        };

        PlayFabClientAPI.UpdateUserData(request, OnSaveSuccess, OnSaveFailure);
    }

    public void OnLoadSuccess(GetUserDataResult result)
    {
        if (result.Data != null && result.Data.ContainsKey("animalName"))
        {
            Debug.Log($"Data loaded: {result.Data["playerName"].Value}");
        }
        else
        {
            Debug.Log("No player data found.");
        }
    }

    void OnLoadFailure(PlayFabError error)
    {
        Debug.LogError($"Error loading player data: {error.GenerateErrorReport()}");
    }

    public void LoadPlayerData(string key)
    {
        var request = new GetUserDataRequest();

        PlayFabClientAPI.GetUserData(request, OnLoadSuccess, OnLoadFailure);
    }

    void OnSaveSuccess(UpdateUserDataResult result)
    {
        Debug.Log("Player data saved successfully.");
    }

    void OnSaveFailure(PlayFabError error)
    {
        Debug.LogError($"Error saving player data: {error.GenerateErrorReport()}");
    }

    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login successful!");
        LoadPlayerData("playerName"); // Load player data after login
    }
    void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError($"Error logging in: {error.GenerateErrorReport()}");
    }
}
