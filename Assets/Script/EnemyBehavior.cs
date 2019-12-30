using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class EnemyBehavior : MonoBehaviour {
    BoxCollider2D Bc;
    SpriteRenderer Sr;
    Rigidbody2D Rb;
    Animator Anim;

    public GameObject PunchEffect;
    public enum Behavior { Move, Hurt, Attack, Death, Idle }
    public GameObject player;
    public Behavior action;

    public float health = 20f;
    private float damage = 5f;
    public float speed;
    public float range = 0.5f;

    public float startTime = 1;
    private float currTime = 0;
    float hitTime = 0;
    private float push;

    bool delay;
    Vector2 relativePoint;
    public Vector2 HitPos;
    public Vector2 HitSize;
    public Vector2 BoxSize;
    public Vector2 BoxPos;
    public bool isHit = false;

    bool isDeath = false;
    bool isAttacking = false;
    bool isFalling = false;


    public Sound[] sounds;

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.Volume;
            s.source.pitch = s.Pitch;
        }
        //HitBox = Resources.Load<GameObject>("Assets/Prefab/HitBox");
    }

    // Use this for initialization
    void Start() {
        Bc = GetComponent<BoxCollider2D>();
        Sr = GetComponent<SpriteRenderer>();
        Rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        action = Behavior.Move;
        currTime = startTime;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (hitTime > 0)
            hitTime -= Time.deltaTime;
        else
        {
            isHit = false;
        }

        if(isHit)
            action = Behavior.Hurt;
        Fall();
        EnemAction();
        if (health <=0)
        {
            isDeath = true;
            action = Behavior.Death;
        }
        
        player = GameObject.FindGameObjectWithTag("Player");
        relativePoint = transform.InverseTransformPoint(player.transform.position);

        //HitDetect();
    }

    public void Fall()
    {
        RaycastHit2D hitInfo;
        Vector2 boxPosition = new Vector2(BoxPos.x + transform.position.x, BoxPos.y + transform.position.y);
        hitInfo = Physics2D.BoxCast(boxPosition, BoxSize, 0, Vector2.down, BoxPos.y);

        if (hitInfo.collider.gameObject.tag == "Ground")
            isFalling = false;
        else
            isFalling = true;

        Anim.SetBool("isFalling", isFalling);
        Anim.SetBool("isDead", isDeath);
    }
    public void EnemAction()
    {
        
        switch (action)
        {
            case Behavior.Move:
                Anim.SetBool("isMoving", true);
                if (relativePoint.x > range)
                {
                    transform.Translate(Vector2.right * speed * Time.deltaTime);
                    Sr.flipX = true;
                }
                else if (relativePoint.x < -range+1 )
                {
                    transform.Translate(Vector2.left * speed*Time.deltaTime);
                    Sr.flipX = false;
                }
                else
                {
                    currTime = startTime;
                    action = Behavior.Idle;
                }
                break;
            case Behavior.Hurt:
                if(!isHit)
                    action = Behavior.Idle;

                break;
            case Behavior.Attack:
                Play("Attack");
                HitDetect();
                if (!isAttacking)
                {
                    Anim.SetTrigger("Attack");
                    isAttacking = true;
                }
                StartCoroutine(actionDelay(0.5f));
                
                break;
            case Behavior.Death:
                Anim.SetTrigger("KO");
                if(!isFalling)
                    Destroy(gameObject,1f);
                break;
            case Behavior.Idle:
                Anim.SetBool("isMoving", false);

                if (currTime > 0)
                {
                    currTime -= Time.deltaTime;
                    if (Vector2.Distance(Sr.transform.position, player.GetComponent<SpriteRenderer>().transform.position) > range || Vector2.Distance(Sr.transform.position, player.GetComponent<SpriteRenderer>().transform.position) < -range)
                    {
                        action = Behavior.Move;
                    }
                }
                else
                {
                    currTime = Random.Range(1f,3f);
                    action = Behavior.Attack;
                }
                break;
        }
    }

    public void HitDetect()
    {
        RaycastHit2D hitInfo;
        Vector2 HitPosition = new Vector2((HitPos.x * EnemyDir()) + transform.position.x, HitPos.y + transform.position.y);
        hitInfo = Physics2D.BoxCast(HitPosition, HitSize, 0, Vector2.right * EnemyDir(), HitPos.x);

        if (hitInfo.collider.tag == "Player")
        {
            if (!hitInfo.collider.GetComponent<Player>().isHit)
            {
                hitInfo.collider.GetComponent<Player>().RecieveDamage(5f, EnemyDir(), 1000f);
                Instantiate(PunchEffect, hitInfo.collider.transform.position, Quaternion.identity);
            }
            isAttacking = true;

        }
        else
        {
            isAttacking = false;
        }
    }

    public void DamageRecieve(float damage, float push,float direction,bool isAir)
    {
        hitTime = 0.1f;
        isHit = true;
        Turn(direction);
        Debug.Log(isAir);
        Play("Hit");
        if (isAir)
        {
            /*if(isFalling)
                Rb.AddForce(Vector2.up * 200f, ForceMode2D.Impulse);
            else
                Rb.AddForce(Vector2.up * push, ForceMode2D.Impulse);*/
            Rb.AddForce(Vector2.up * push, ForceMode2D.Impulse);
        }
        else
            Rb.AddForce(Vector2.right * direction * push, ForceMode2D.Impulse);

        if (!isDeath)
        {
            health -= damage;
            action = Behavior.Hurt;
            Play("Hurt");
            Anim.SetTrigger("Hurt");
        }
        else
        {
            Play("Death");
            action = Behavior.Death;
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 boxPosition = new Vector2(BoxPos.x + transform.position.x, BoxPos.y + transform.position.y);
       // Vector2 HitPosition = new Vector2((HitPos.x * EnemyDir()) + transform.position.x, HitPos.y + transform.position.y);
        if (isFalling)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;
        Gizmos.DrawCube(boxPosition, BoxSize);

        if (isAttacking)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;
        //Gizmos.DrawWireCube(HitPosition, HitSize);
    }
    public float EnemyDir()
    {
        if (Sr.flipX == true)
            return -1f;
        else
            return 1f;

    }
    public void Turn(float direction)
    {
        if (direction > 0)
            Sr.flipX = true;
        else
            Sr.flipX = false;
    }

    public float getHealth()
    {
        return health;
    }

    IEnumerator actionDelay(float time)
    {
        yield return new WaitForSeconds(time);
        isAttacking = false;
        action = Behavior.Idle;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

}
