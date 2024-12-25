using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class ClientandServerManager : Singleton<ClientandServerManager>
{
    // Start is called before the first frame update
    public Server server;
    public Client client;
    public GameObject player1;
    public GameObject player2;
    public GameObject stage;
    int count = 0;
    void Start()
    {
        player1.GetComponent<MovementController>().inputUp = KeyCode.None;
        player1.GetComponent<MovementController>().inputDown = KeyCode.None;
        player1.GetComponent<MovementController>().inputLeft = KeyCode.None;
        player1.GetComponent<MovementController>().inputRight = KeyCode.None;
        player2.GetComponent<MovementController>().inputUp = KeyCode.None;
        player2.GetComponent<MovementController>().inputDown = KeyCode.None;
        player2.GetComponent<MovementController>().inputLeft = KeyCode.None;
        player2.GetComponent<MovementController>().inputRight = KeyCode.None;

    }
    // Update is called once per frame
    void Update()
    {
        if(client.step == Client.STEP.START)
        {
            count++;
            Debug.Log("csupdate"+count);
            if(count>10)
            {
                if (client.me == 1)
                {
                    string Message = "play,1," + player1.transform.position.x + "," + player1.transform.position.y + "," + player1.GetComponent<MovementController>().directionnum
                        + "/";
                    server.BroadcastMessage(Message);
                }
                if (client.me == 2)
                {
                    string Message = "play,2," + player2.transform.position.x + "," + player2.transform.position.y + "," + player2.GetComponent<MovementController>().directionnum + "/";
                    client.SendMessage(Message);

                }
                count = 0;
            }
            else
            {
                if (client.me == 1)
                {
                    string Message = "play,1," + player1.GetComponent<MovementController>().directionnum
                        + "/";
                    server.BroadcastMessage(Message);
                    

                }
                else if (client.me == 2)
                {
                    string Message = "play,2,"+player2.GetComponent<MovementController>().directionnum + "/";
                    client.SendMessage(Message);
                    //if (player1.GetComponent<MovementController>().directionnum > 0)
                    //{
                    //    switch (player1.GetComponent<MovementController>().directionnum)
                    //    {
                    //        case 1:
                    //            player1.GetComponent<MovementController>().SetDirection(Vector2.up, player1.GetComponent<MovementController>().spriteRendererUp);
                    //            break;
                    //        case 2:
                    //            player1.GetComponent<MovementController>().SetDirection(Vector2.down, player1.GetComponent<MovementController>().spriteRendererUp);
                    //            break;
                    //        case 3:
                    //            player1.GetComponent<MovementController>().SetDirection(Vector2.left, player1.GetComponent<MovementController>().spriteRendererUp);
                    //            break;
                    //        case 4:
                    //            player1.GetComponent<MovementController>().SetDirection(Vector2.right, player1.GetComponent<MovementController>().spriteRendererUp);
                    //            break;

                    //    }

                    //}
                }
            }
        }

    }
    public void ParseMessage(string message)
    {
        Debug.Log("开始解析" + message);
        string[] parts = message.Split('/');
        foreach (string part in parts)
        {
            if (string.IsNullOrEmpty(part)) continue;

            string[] subParts = part.Split(',');
            if (subParts.Length < 2) continue;

            Debug.Log("信息:" + part);
            switch (subParts[0])
            {
                case "play":
                    if (subParts.Length>=5)
                    {
                        Debug.Log("player信息:");
                        int playerId = int.Parse(subParts[1]);
                        float x = float.Parse(subParts[2]);
                        float y = float.Parse(subParts[3]);
                        int directionNum = int.Parse(subParts[4]);
                        if(playerId==1)
                        {
                            player1.transform.position = new Vector3(x, y, 0);
                            if(client.me !=playerId)
                            {
                                player1.GetComponent<MovementController>().directionnum = directionNum;
                            }
                            Debug.Log(player1.transform.position.ToString() + " " + player1.GetComponent<MovementController>().directionnum);
                        }
                        if (playerId == 2)
                        {
                            player2.transform.position = new Vector3(x, y, 0);
                            if (client.me != playerId)
                            {
                                player2.GetComponent<MovementController>().directionnum = directionNum;
                            }
                            Debug.Log(player2.transform.position.ToString() + " " + player2.GetComponent<MovementController>().directionnum);
                        }
                    }
                    else
                    {
                        int playerId = int.Parse(subParts[1]);
                        int directionNum = int.Parse(subParts[2]);
                        if (playerId == 1)
                        {
                            if (client.me != playerId)
                            {
                                player1.GetComponent<MovementController>().directionnum = directionNum;
                                Debug.Log(player1.GetComponent<MovementController>().directionnum);
                            }
                        }
                        if (playerId == 2)
                        {
                            if (client.me != playerId)
                            {
                                player2.GetComponent<MovementController>().directionnum = directionNum;
                                Debug.Log(player1.GetComponent<MovementController>().directionnum);
                                //if (player2.GetComponent<MovementController>().directionnum > 0)
                                //{
                                //    switch (player2.GetComponent<MovementController>().directionnum)
                                //    {
                                //        case 1:
                                //            player2.GetComponent<MovementController>().SetDirection(Vector2.up, player2.GetComponent<MovementController>().spriteRendererUp);
                                //            break;
                                //        case 2:
                                //            player2.GetComponent<MovementController>().SetDirection(Vector2.down, player2.GetComponent<MovementController>().spriteRendererUp);
                                //            break;
                                //        case 3:
                                //            player2.GetComponent<MovementController>().SetDirection(Vector2.left, player2.GetComponent<MovementController>().spriteRendererUp);
                                //            break;
                                //        case 4:
                                //            player2.GetComponent<MovementController>().SetDirection(Vector2.right, player2.GetComponent<MovementController>().spriteRendererUp);
                                //            break;

                                //    }

                                //}
                            }
                        }
                    }
                    break;
                case "bomb":
                    if (subParts.Length >= 3)
                    {
                        float x = float.Parse(subParts[1]);
                        float y = float.Parse(subParts[2]);
                        float id = float.Parse(subParts[3]);
                    }
                    break;
                default:
                    Debug.Log("无效信息");
                    break;
            }
        }
    }
}
