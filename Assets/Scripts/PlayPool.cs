using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayPool : MonoBehaviour
{
    public GameObject preFab;
    //×Öµä
    Dictionary<string, GameObject> modles = new Dictionary<string, GameObject>();
    
    
    public void updatePlayer(List<PlayerInfo> list) 
    {
        foreach (PlayerInfo p  in list) 
        {
            if (modles.ContainsKey(p.name))
            {
                modles[p.name].transform.position = new Vector3(p.x, p.y);
            }
            else
            {
                Instantiate(preFab, new Vector3(p.x, p.y), Quaternion.identity);
            }
        }
        
    }

    public static PlayPool ins;

    private void Awake()
    {
        ins = this;
    }
}
