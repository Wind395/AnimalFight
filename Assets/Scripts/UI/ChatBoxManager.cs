using System.Collections.Generic;
using ExitGames.Client.Photon;
using Fusion;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class ChatBoxManager : NetworkBehaviour, IChatClientListener
{

    public static ChatBoxManager Instance;
    private List<string> chatMessages = new List<string>();
    public ChatUI chatUI;
    ChatClient chatClient;
    public bool isChatOpen = false;
    
    public AnimalProperties animalProperties;

    private void Awake()
    {
        Instance = this;

        PhotonNetwork.LocalPlayer.NickName = "Player" + Random.Range(1, 1000);
    }

    void Start() 
    {

        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion, new AuthenticationValues($"User{PhotonNetwork.LocalPlayer.UserId}"));
    }

    public override void FixedUpdateNetwork()
    {
        
    }

    void Update()
    {
        chatClient.Service();

        animalProperties = GameObject.FindGameObjectWithTag("Player").GetComponent<AnimalProperties>();
        if (animalProperties == null)
        {
            Debug.Log("AnimalProperties not found. Cannot send chat message.");
        }
        
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcReceiveMessage(string playerName, string message)
    {
        string formattedMessage = $"{playerName}: {message}";
        chatMessages.Add(formattedMessage);
        chatUI.chatTextDisplay.text += formattedMessage + "\n";
    }

    public void SendChatMessage(string message)
    {
        if (animalProperties != null)
        {
            //string playerName = Runner.LocalPlayer.PlayerId.ToString();
            string playerName = animalProperties.animalName;

            RpcReceiveMessage(playerName, message);

            chatClient.PublishMessage("General", message);
        }
        else
        {
            Debug.Log("AnimalProperties not found. Cannot send chat message.");
        }
        
    }

    public void OnConnected()
    {
        chatClient.Subscribe(new string[] { "General" });
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log("Chat state changed: " + state);
    }

    public void OnGetMessage(string channelName, string[] sender, object[] message)
    {
        string msg = "";
        for (int i = 0; i < sender.Length; i++)
        {
            msg += $"{sender[i]}: {message[i]}\n";
            chatUI.chatTextDisplay.text += msg + "\n";
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnDisconnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        //throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels)
    {
        //throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }
}
