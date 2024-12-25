using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] players;

    public void CheckWinState()
    {
        int aliveCount = 0;
        
        //这是啥？？？
        foreach (GameObject player in players)
        {
            if (player.activeSelf)
            {
                aliveCount++; 
            }
        }

        if(aliveCount <=1)
        {
            Invoke(nameof(Newround),3f);
        }

        
    }

    private void Newround()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
