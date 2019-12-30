using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class HitBox : MonoBehaviour {
    public float xDirection=0;
    public float yDirection=0;
    public bool isPlayer = false;
    public GameObject GameManager;
    public GameObject HitEffect;
    Vector3 HitPos;
    public float Damage;
    float currTime = 0.5f;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //transform.position = new Vector3(xDirection, yDirection, 0);
        HitPos = transform.position;
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (isPlayer)
        {
            if (col.gameObject.tag == "Enemy")
            {
                //col.gameObject.GetComponent<EnemyBehavior>().DamageRecieve(5f);
                GameManager.GetComponent<ComboSystem>().ComboAdd();
                Instantiate(HitEffect, HitPos, Quaternion.identity);
            }
        }
        else
        {
            if (col.gameObject.tag == "Player")
            {
                //col.gameObject.GetComponent<Player>().RecieveDamage(Damage);
                Instantiate(HitEffect, HitPos/*new Vector2(col.transform.position.x, col.transform.position.y + 1.8f)*/, Quaternion.identity);
            }
        }
    }
    
}
