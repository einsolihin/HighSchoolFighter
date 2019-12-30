using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectScript : MonoBehaviour {
    Animator Anim;
    // Use this for initialization
    void Start () {
        Anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Destroy(gameObject, Anim.GetCurrentAnimatorStateInfo(0).length);
	}
}
