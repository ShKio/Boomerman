using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destrucible : MonoBehaviour
{
    public float destructionTime = 1f;

    [Range(0f, 1f)]
    public float itemSpawnChance = 0.2f;

   
    public GameObject[] spawnableItem;


    private void Start()
    {
        Destroy(gameObject,destructionTime);        
    }


    // 记得复习UNITY运行周期的函数相关
    private void OnDestroy()
    {
        if (spawnableItem.Length >0 && Random.value < itemSpawnChance)
        {
            int randomIndex = Random.Range(0, spawnableItem.Length);
            Instantiate(spawnableItem[randomIndex], transform.position, Quaternion.identity);

        }
    }

}
