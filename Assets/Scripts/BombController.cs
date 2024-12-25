using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    public List<GameObject> bombs = new List<GameObject>();
    [Header("Bomb")]
    public GameObject bombPrefab;
    public KeyCode inputKey = KeyCode.Space;
    public float bombFuseTime = 3f;
    public int bombAmount = 1;
    private int bombsRemaining;

    //爆炸相关设置
    //标头语法 是啥？

    [Header("Explosion")]
    public Explosion explosionPrfab;
    public LayerMask explosionMask;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    [Header("Destrucible")]
    public Destrucible destruciblePrefab;
    public Tilemap destrucibleTiles;

    private void OnEnable()
    {
        bombsRemaining = bombAmount;
    }

    private void Update()
    {
        if(bombsRemaining > 0 && Input.GetKeyDown(inputKey))
        {
            StartCoroutine(PlaceBomb());
        }
    }


    private IEnumerator PlaceBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        //创建一个新的 bombPrefab 预设对象。
        //将新对象放置在 position 指定的位置。
        //将新对象的旋转设置为默认的零旋转。
        bombs.Add(bomb);
        bombsRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        //推炸弹
        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        Explosion explosion = Instantiate(explosionPrfab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.Start);
        explosion.DestroyAfter(explosionDuration);
        //Destroy(explosion.gameObject, explosionDuration);

        Explode(position,Vector2.up, explosionRadius);
        Explode(position,Vector2.down, explosionRadius);
        Explode(position,Vector2.left, explosionRadius);
        Explode(position,Vector2.right, explosionRadius);
        bombs.Remove(bomb);
        Destroy(bomb);
        bombsRemaining++;

    }

    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if(length <= 0)
        {
            return;
        }
        position += direction;

        //检测砖块
        //返回一个碰撞快？
        if (Physics2D.OverlapBox(position , Vector2.one / 2f , 0f , explosionMask))
        {
            ClearDestrucible(position);
            return;
        }


        //爆炸的蔓延
        Explosion explosion = Instantiate(explosionPrfab, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion.Middle : explosion.End);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);
        //Destroy(explosion.gameObject, explosionDuration);

        Explode(position, direction, length - 1);

    }


   //TODO
    private void ClearDestrucible(Vector2 position)
    {
        Vector3Int cell = destrucibleTiles.WorldToCell(position);
        TileBase tile = destrucibleTiles.GetTile(cell);
        if (tile != null)
        {
           Instantiate(destruciblePrefab,position,Quaternion.identity);
            destrucibleTiles.SetTile(cell,null);
        }
    }


    public void AddBomb()
    {
        bombAmount++;
        bombsRemaining++;
    }

    //设置可以推炸弹的函数
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = false;
        }
    }


}
