using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using TMPro;

public class Client : MonoBehaviour
{
    public enum STEP
    {
        BEFORECONNECT, CONNECT, START
    }
    public TMP_InputField hostInputField;
    public TMP_InputField portInputField;
    public TMP_InputField nameInputField;
    public TMP_InputField messageInputField;
    public TMP_Text chatBox;
    public int me = 0;
    private TcpClient client;
    private NetworkStream stream;
    private Thread clientThread;
    public STEP step;
    public void Start()
    {
        step = STEP.BEFORECONNECT;

    }
    public void ConnectToServer()
    {
        string host = hostInputField.text;
        int port = int.Parse(portInputField.text);

        client = new TcpClient();
        client.Connect(host, port);
        stream = client.GetStream();
        clientThread = new Thread(ListenForData);
        clientThread.Start();
    }

    private void ListenForData()
    {
        byte[] buffer = new byte[1024];
        int bytesRead;

        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Debug.Log("客户端接受：" + message);
            switch (step)
            {
                case STEP.BEFORECONNECT:
                    me = int.Parse(message);
                    MainThreadDispatcher.ExecuteOnMainThread(() => keyplayer());
                    if (me!=0)
                    {
                        step = STEP.CONNECT;
                        string playerName = nameInputField.text;
                        SendMessage(playerName + " has joined the chat");
                    }
                    break;
                case STEP.CONNECT:
                    if (message.Equals("/start"))
                    {
                        step = STEP.START;
                        break;
                    }
                    MainThreadDispatcher.ExecuteOnMainThread(() => AppendToChatBox(message));
                    break;
                case STEP.START:
                    Debug.Log("客户端游戏开始环节");
                    MainThreadDispatcher.ExecuteOnMainThread(() => ClientandServerManager.Instance.ParseMessage(message));
                    break;
            }
        }
    }
    public void SendMessage(string message)
    {
        if (stream != null&&step == STEP.CONNECT)
        {
            Debug.Log("客户端发送：" + message);
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }
        else if (stream != null && step == STEP.START)
        {
            Debug.Log("客户端发送：" + message);
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

    }
    public void senddata()
    {
        SendMessage("");
    }

    private void AppendToChatBox(string message)
    {
        Debug.Log("消息写上留言板"+me);
        chatBox.text += message + "\n";
    }
    public void keyplayer()
    {
        if (me == 1)
        {
            Debug.Log("是玩家1");
            ClientandServerManager.Instance.player1.GetComponent<MovementController>().inputUp = KeyCode.W;
            ClientandServerManager.Instance.player1.GetComponent<MovementController>().inputDown = KeyCode.S;
            ClientandServerManager.Instance.player1.GetComponent<MovementController>().inputLeft = KeyCode.A;
            ClientandServerManager.Instance.player1.GetComponent<MovementController>().inputRight = KeyCode.D;
        }
        if (me == 2)
        {
            Debug.Log("是玩家2");
            ClientandServerManager.Instance.player2.GetComponent<MovementController>().inputUp = KeyCode.W;
            ClientandServerManager.Instance.player2.GetComponent<MovementController>().inputDown = KeyCode.S;
            ClientandServerManager.Instance.player2.GetComponent<MovementController>().inputLeft = KeyCode.A;
            ClientandServerManager.Instance.player2.GetComponent<MovementController>().inputRight = KeyCode.D;
        }
    }
    void OnDestroy()
    {
        client.Close();
        stream.Close();
        clientThread?.Abort();
    }
}