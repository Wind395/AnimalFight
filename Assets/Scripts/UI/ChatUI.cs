using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ChatUI : MonoBehaviour
{

    public TMP_InputField chatText;
    public Button sendButton;
    public TextMeshProUGUI chatTextDisplay;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sendButton.onClick.AddListener(SendMessage);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && ChatBoxManager.Instance.isChatOpen)
        {
            ChatBoxManager.Instance.isChatOpen = false;
            //chatText.gameObject.SetActive(false);
            SendMessage();
        }

        if (Input.GetKeyDown(KeyCode.Return) && !ChatBoxManager.Instance.isChatOpen)
        {
            ChatBoxManager.Instance.isChatOpen = true;
            //chatText.gameObject.SetActive(true);
            chatText.Select();
            chatText.ActivateInputField();
        }
    }

    private void SendMessage()
    {
        string message = chatText.text;
        if (!string.IsNullOrEmpty(message))
        {
            ChatBoxManager.Instance.SendChatMessage(message);
            chatText.text = string.Empty; // Clear the input field after sending
        }
    }
}
