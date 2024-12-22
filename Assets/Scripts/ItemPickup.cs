using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    //枚举出三个物品
    //根据物品的不同来实现不同的功能
    public enum ItemType
    {
        ExtraBomb,
        BlastRadius,
        SpeedIncrease,

    }

    public ItemType Type;

    private void OnItemPickUp(GameObject player)
    {
        switch(Type)
        {
            case ItemType.ExtraBomb:
                player.GetComponent<BombController>().AddBomb();
                break;
             case ItemType.BlastRadius:
                player.GetComponent<BombController>().explosionRadius ++;

                break;
            case ItemType.SpeedIncrease:
                player.GetComponent<MovementController>().speed ++;
                break;
        }

        Destroy(gameObject);
    }

    //碰撞器
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnItemPickUp(other.gameObject);
        }
    }

}
