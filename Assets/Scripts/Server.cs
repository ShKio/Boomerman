using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using TMPro;

public class Server : MonoBehaviour
{
    public enum ServerSTEP
    {
        BEFORESTART, START
    }
    public TMP_InputField portInputField;
    public TMP_Text chatBox;
    public TcpListener server;
    public List<TcpClient> clients;
    public Thread serverThread;
    private bool isRunning = false;
    public ServerSTEP serverSTEP;
    void Start()
    {
        clients = new List<TcpClient>();
    }

    public void StartServer()
    {
        int port = int.Parse(portInputField.text);
        server = new TcpListener(IPAddress.Any, port);
        server.Start();
        isRunning = true;
        serverThread = new Thread(RunServer);
        serverThread.Start();
        AppendToChatBox("Server started on port " + port);
    }

    private void RunServer()
    {
        while (isRunning)
        {
            if (server.Pending())
            {
                TcpClient client = server.AcceptTcpClient();
                lock (clients)
                {
                    clients.Add(client);
                    SendMessage(clients.Count.ToString(),client);
                }
                Thread clientThread = new Thread(HandleClient);
                clientThread.Start(client);
            }
        }
    }

    private void HandleClient(object clientObj)
    {
        TcpClient client = (TcpClient)clientObj;
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;

        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            if(serverSTEP==ServerSTEP.BEFORESTART)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                MainThreadDispatcher.ExecuteOnMainThread(() => AppendToChatBox(message));
                BroadcastMessage(message);
            }
            else
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                MainThreadDispatcher.ExecuteOnMainThread(() => AppendToChatBox(message));
                BroadcastMessage(message);
            }
        }

        lock (clients)
        {
            clients.Remove(client);
        }
        client.Close();
    }

    public void BroadcastMessage(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        lock (clients)
        {
            foreach (TcpClient client in clients)
            {
                Debug.Log("服务端发送："+message);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
            }
        }
    }
    private void SendMessage(string message,TcpClient client)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        Debug.Log("服务端发送：" + message+"给"+client.ToString());
        NetworkStream stream = client.GetStream();
        stream.Write(data, 0, data.Length);
    }

    private void AppendToChatBox(string message)
    {
        chatBox.text += message + "\n";
    }

    void OnDestroy()
    {
        isRunning = false;
        server.Stop();
        serverThread?.Abort();
    }
}