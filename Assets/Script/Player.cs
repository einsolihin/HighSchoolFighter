using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Player : MonoBehaviour {
    Rigidbody2D PlayerRB;
    BoxCollider2D PlayerBC;
    SpriteRenderer PlayerSR;
    Animator PlayerAnim;
    Swipe swipe;
    public Joystick joystick;
    public GameObject ParticleSystem;
    public float speed;
    public float JumpForce;
    public float SideForce;
    public float playerHealth = 100f;
    public float damage = 10f;
    float HorizontalMove = 1f;
    float attackCombo = 0;

    public Vector2 boxSize = new Vector2(1, 0.25f);
    public Vector2 hitSize = new Vector2(1, 0.25f);
    public Vector2 BoxPos = new Vector2(1, 0.25f);
    public Vector2 HitPos = new Vector2(1, 0.25f);

    public GameObject EventSystem;

    //attack action
    bool PlayerAction = false;
    bool isFalling = false;
    bool isAttacking=false;
    public bool isHit = false;
    float Direction = 1;
    bool isDead = false;
    float attPush = 200f;
    bool isAirAttack = false;
    bool gotHit = false;

    //Set Timer
    float timeStart = 0;
    float hitTime = 1f;

    public Sound[] sounds;

    private void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.Volume;
            s.source.pitch = s.Pitch;
        }
    }

    // Use this for initialization
    void Start () {
        PlayerRB = GetComponent<Rigidbody2D>();
        PlayerBC = GetComponent<BoxCollider2D>();
        PlayerSR = GetComponent<SpriteRenderer>();
        PlayerAnim = GetComponent<Animator>();
        swipe = GetComponent<Swipe>();
        PlayerSR.flipX = false;
        timeStart = 0;
        isDead = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Movement();
        if (!isDead)
            Action();

        PlayerAnim.SetBool("isAttacking", isAttacking);

        if (timeStart > 0)
        {
            HitDetect();
            timeStart -= Time.deltaTime;
        }
        else
        {
            isAttacking = false;
            attackCombo = 0;
        }

        if (hitTime > 0)
            hitTime -= Time.deltaTime;
        else
            isHit = false;

        if (PlayerRB.velocity.y < -1)
            isFalling = true;
        else
            isFalling = false;
        
    }

    public void RecieveDamage(float damage,float dir, float push)
    {
        
        EventSystem.GetComponent<UI>().PlayerHurt();
        playerHealth -= damage;
        PlayerRB.AddForce(dir * push * Vector2.left, ForceMode2D.Impulse);
        isHit = true;
        hitTime = 1f;
        Play("Hit");
        if (playerHealth>0)
        {
            Play("Hurt");
            PlayerAnim.SetTrigger("Hurt");
        }
        else
        {
            if(!isDead )
                Play("KO");
            PlayerAnim.SetTrigger("KO");
            isDead = true;
        }


    }

    #region Attack

    public void Action()
    {
        if (swipe.Tap||Input.GetKeyDown(KeyCode.P))
        {
            // if(attackCombo <4)
            if(!PlayerAnim.GetCurrentAnimatorStateInfo(0).IsName("PlayerA4") && !PlayerAnim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAA4"))
            {
                if (isFalling)
                {
                    isAirAttack = true;
                    attPush = 800f;
                    PlayerRB.AddForce(Vector2.up * 800, ForceMode2D.Impulse);
                }
                else
                {
                    attPush = 200f;
                    isAirAttack = false;
                }
                attackCombo++;
                timeStart = 0.1f;
                PlayerAnim.SetTrigger("Attack");
                //isAttacking = true;
                
            }
           
        }
        else if (swipe.SwipeRight)
        {
            isAirAttack = false;
            attPush = 200f;
            PlayerSR.flipX = true;
            PlayerAnim.SetTrigger("Slash");
            PlayerRB.AddForce(Vector2.right * SideForce, ForceMode2D.Impulse);
            timeStart = 0.1f;
        }
        else if (swipe.SwipeLeft)
        {
            isAirAttack = false;
            attPush = 200f;
            PlayerSR.flipX = false;
            PlayerAnim.SetTrigger("Slash");
            PlayerRB.AddForce(Vector2.left * SideForce, ForceMode2D.Impulse);
            timeStart = 0.1f;
        }
        else if (swipe.SwipeDown)
        {
            
            if (isFalling)
            {
                isAirAttack = true;
                attPush = -5000f;
                PlayerAnim.SetTrigger("DownSlash");
                PlayerRB.AddForce(Vector2.down * JumpForce, ForceMode2D.Impulse);
                timeStart = 0.1f;
                
            }
        }
        else if (swipe.SwipeUp)
        {
            
            if (!isFalling && !(timeStart>0))
            {
                isAirAttack = true;
                attPush = 8000f;
                PlayerAnim.SetTrigger("UpperSlash");
                PlayerRB.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
                timeStart = 0.1f;
                Play("Jump");

            }
        }

        
    }
    public void HitDetect()
    {
        isAttacking = true;
        RaycastHit2D hitInfo;
        Vector2 HitPosition = new Vector2((HitPos.x * PlayerDir()) + transform.position.x, HitPos.y + transform.position.y);
        hitInfo = Physics2D.BoxCast(HitPosition, hitSize, 0, Vector2.right * PlayerDir(), HitPos.x);
        Debug.Log("Draw");
        if (hitInfo.collider.tag == "Enemy")
        {

            if (!hitInfo.collider.GetComponent<EnemyBehavior>().isHit)
            {
                hitInfo.collider.GetComponent<EnemyBehavior>().DamageRecieve(5f, attPush, PlayerDir(), isAirAttack);
                Debug.Log("Touch");
                EventSystem.GetComponent<ComboSystem>().ComboAdd();
                gotHit = true;
                Instantiate(ParticleSystem, hitInfo.collider.transform);
                Debug.Log("is a Hit");
            }
            else
            {
                gotHit = false;
            }
            
        }
    }

    IEnumerator actionDelay(float time)
    {
        //HitDetect();
        yield return new WaitForSeconds(time);
        gotHit = false;
        PlayerAnim.SetTrigger("Stop");
        Physics2D.IgnoreLayerCollision(8, 9, false);
    }
    #endregion


    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    private void OnDrawGizmos()
    {
        
        Vector2 boxPosition = new Vector2(BoxPos.x + transform.position.x, BoxPos.y + transform.position.y);
        Vector2 HitPosition = new Vector2((HitPos.x * PlayerDir()) + transform.position.x, HitPos.y + transform.position.y);
        if(isFalling)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;
        Gizmos.DrawCube(boxPosition, boxSize);

        if (gotHit)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;
        Gizmos.DrawWireCube(HitPosition, hitSize);
    }

    #region Movement

    public float PlayerDir()
    {
        if (PlayerSR.flipX == true)
            return -1f;
        else
            return 1f;

    }
    public void Movement()
    {
        // Detect if player is Falling
        RaycastHit2D hitInfo;
        Vector2 boxPosition = new Vector2(BoxPos.x + transform.position.x, BoxPos.y + transform.position.y);
        hitInfo = Physics2D.BoxCast(boxPosition, boxSize, 0, Vector2.down, BoxPos.y);

        if (hitInfo.collider.gameObject.tag == "Ground")
            isFalling = false;
        else
            isFalling = true;

        PlayerAnim.SetBool("isFalling", isFalling);


        //Player Movement
        HorizontalMove = joystick.Horizontal * speed;
        if (joystick.Horizontal > 0.75 || joystick.Horizontal < -0.75)
        {
            PlayerRB.AddForce(Vector2.right * speed * HorizontalMove);
        }
        PlayerAnim.SetFloat("Speed", Mathf.Abs(PlayerRB.velocity.x));

        if (PlayerRB.velocity.x < -0.7 && PlayerSR.flipX)
        {
            PlayerBC.offset = new Vector2(-PlayerBC.offset.x, PlayerBC.offset.y);
            if (isHit)
                PlayerSR.flipX = true;
            else
                PlayerSR.flipX = false;
        }
        else if (PlayerRB.velocity.x > 0.7 && !PlayerSR.flipX)
        {
            PlayerBC.offset = new Vector2(-PlayerBC.offset.x, PlayerBC.offset.y);
            if(isHit)
                PlayerSR.flipX = false;
            else
                PlayerSR.flipX = true;
        }
        
    }
    

    #endregion
}
