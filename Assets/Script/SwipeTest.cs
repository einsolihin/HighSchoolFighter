using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeTest : MonoBehaviour {
    Swipe swipe;
    Camera cam;
    public Text tx;
    public Text tx2;
    public GameObject pEffect;
    // Use this for initialization
    void Start () {
        cam = Camera.main;
        swipe = GetComponent<Swipe>();
        
	}
	
	// Update is called once per frame
	void Update () {
        float x = Screen.width;
        string s ;//"Screen Size : " + x.ToString();
        tx2.text = "Screen Size : " + x.ToString();
        if (swipe.Tap)
        {
            tx.text = "Tap";
            Vector3 v3 = cam.ScreenToWorldPoint(swipe.StartTouch);
            Instantiate(pEffect, v3, Quaternion.identity);
            Debug.Log(swipe.StartTouch);
        }
        if (swipe.SwipeUp)
        {
            tx.text = "Swipe Up";
            Debug.Log("Swipe Up!");
        }
        if (swipe.SwipeDown)
        {
            tx.text = "Swipe Down";
            Debug.Log("Swipe Down!");
        }
        if (swipe.SwipeRight)
        {
            tx.text = "Swipe Right";
            Debug.Log("Swipe Right!");
        }
        if (swipe.SwipeLeft)
        {
            tx.text = "Swipe Left";
            Debug.Log("Swipe Left!");
        }
    }
    public void TouchEffect()
    {
        if (swipe.Touch)
        {
            Debug.Log(swipe.StartTouch);
            //transform.position =swipe.StartTouch();
        }
    }
}
