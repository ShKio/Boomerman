using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    private Vector2 direction = Vector2.down;
    public float speed = 5f;
    public KeyCode inputUp = KeyCode.None;
    public KeyCode inputDown = KeyCode.None;
    public KeyCode inputLeft = KeyCode.None;
    public KeyCode inputRight = KeyCode.None;
    public int directionnum=0;
    //预设的四个方向的动画
    public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererRight;


    public AnimatedSpriteRenderer spriteRendererDeath;

    //将要显示的动画
    private AnimatedSpriteRenderer activeSpriteRenderer;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        //设置初始化的动画，向下
        activeSpriteRenderer = spriteRendererDown;
    }

    private void Update()
    {
        if (Input.GetKey(inputUp))
        {
            SetDirection(Vector2.up,spriteRendererUp);
            directionnum = 1;
}
        else if (Input.GetKey(inputDown))
        {
            SetDirection(Vector2.down,spriteRendererDown);
            directionnum = 2;
        }
        else if (Input.GetKey(inputLeft))
        {
            //Debug.Log("左");
            SetDirection(Vector2.left,spriteRendererLeft);
            directionnum = 3;
        }
        else if (Input.GetKey(inputRight))
        {
            //Debug.Log("右");

            SetDirection(Vector2.right,spriteRendererRight);
            directionnum = 4;
        }
        else
        {
            if(directionnum>0)
            {
                switch(directionnum)
                {
                    case 1:
                        SetDirection(Vector2.up, spriteRendererUp);
                        break;
                    case 2:
                        SetDirection(Vector2.down, spriteRendererUp);
                        break;
                    case 3:
                        SetDirection(Vector2.left, spriteRendererUp);
                        break;
                    case 4:
                        SetDirection(Vector2.right, spriteRendererUp);
                        break;

                }

            }
            SetDirection(Vector2.zero,activeSpriteRenderer);
            directionnum = 0;
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = direction* speed* Time.fixedDeltaTime;

        rigidbody.MovePosition(position + translation);
    }

    public void SetDirection(Vector2 newDirection,AnimatedSpriteRenderer spriteRenderer)
    {
        direction = newDirection;

        //设置该选择哪个动画播放
        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        //展示动画
        activeSpriteRenderer = spriteRenderer;
        activeSpriteRenderer.idle = direction == Vector2.zero;
    }


    private void OnTriggerEnter2D(Collider2D other)

    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        enabled = false;
        GetComponent<BombController>().enabled = false;

        spriteRendererUp.enabled=false;
        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererDeath.enabled = true;

        Invoke(nameof(OnDeathSequenceEnded), 1f);
    }


    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
        FindObjectOfType<GameManager>().CheckWinState();
    }    



}
