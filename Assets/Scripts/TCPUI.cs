using UnityEngine;

public class UILogic : MonoBehaviour
{
    public Server server;
    public Client client;
    public GameObject game;
    public GameObject UI;

    public void OnHostButtonClick()
    {
        server.StartServer();
        client.hostInputField.text = "127.0.0.1";
        client.portInputField.text = server.portInputField.text;
        if(client.nameInputField.text.Equals(""))
        {
            client.nameInputField.text = "房主";
        }
        client.me = 1;
        client.ConnectToServer();
    }

    public void OnConnectButtonClick()
    {
        client.ConnectToServer();
    }

    public void OnSendButtonClick()
    {
        string message = client.nameInputField.text + ": " + client.messageInputField.text;
        client.SendMessage(message);
        client.messageInputField.text = "";
    }
    public void startgame()
    {
        if(server.clients.Count>1&&client.me==1)
        {
            UI.SetActive(false);
            game.SetActive(true);
            server.BroadcastMessage("/start");
        }
        else
        {
            client.SendMessage("人数不足,或者你不是房主，无法开始游戏");
        }
    }
    public void Start()
    {
        game.SetActive(false);
    }
    public void Update()
    {
        if (client.step == Client.STEP.START)
        {
            UI.SetActive(false);
            game.SetActive(true);
        }
    }
}
