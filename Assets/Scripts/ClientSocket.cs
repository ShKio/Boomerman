using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using Newtonsoft.Json;

public class PlayerInfo
{
    public  string name;
    public float x, y;
    //public
    public PlayerInfo(string n, float a, float b)
    {
        name=n;
        x = a;
        y = b;
    }
}

public class Message
{
    public string type;
    public string info;
    public Message(string type, string info)
    {
        this.type = type;
        this.info = info;
    }
}


public class ClientSocket : MonoBehaviour
{
    static Socket  socket;
    private void Start()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect("127.0.0.1", 8888);
        Listener.StartListen(socket);
    }

    public static void SendMessage(Message msg)
    {
        //PlayerInfo p = new PlayerInfo(gameObject.name, GetComponent<Rigidbody>().position.x, GetComponent<Rigidbody>().position.y);

        PlayerInfo p = new PlayerInfo("111", -6, -5);
        string str = JsonConvert.SerializeObject(p);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str+'&');
        if(socket != null && socket.Connected)
        {
            socket.Send(bytes);
        }
        
    }


}
