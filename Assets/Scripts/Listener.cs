using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading;

public class Listener : MonoBehaviour
{
    public static void StartListen(Socket sc)
    {
        Thread listen = new Thread(new ParameterizedThreadStart(listenServer));
        listen.Start(sc);
    }

    static Queue<Message> toDoList = new Queue<Message>();


    static void listenServer(object obj)
    {
        Socket sclient = (Socket)obj;
        byte[] readbuff = new byte[1024];
        while(true)
        {
            try
            {
                int len = sclient.Receive(readbuff);
                string str = System.Text.Encoding.UTF8.GetString(readbuff, 0, len);

                foreach (string s in str.Split('&'))
                {
                    if (s.Length > 0)
                    {
                        Message msg = JsonConvert.DeserializeObject<Message>(s);
                        toDoList.Enqueue(msg);
                    }

                }
            }
            catch (SocketException)
            {
                break;
            }

        }

       

    }

    private void Update()
    {
        if(toDoList.Count > 0)
        {
            Message msg = toDoList.Dequeue();
            switch (msg.type)
            {
                case "AllPlayerInfo":
                    List<PlayerInfo> listinfo = JsonConvert.DeserializeObject<List<PlayerInfo>>(msg.info);
                    //bug
                    PlayPool.ins.updatePlayer(listinfo);
                    break;
            }
        }
    }
}
