using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineEffect : MonoBehaviour {
    bool isDragging = false;
    float Xrange = 0;
    Vector2 startTouch;
    Rigidbody2D rb;
    Camera cam;
    TrailRenderer TR;
    public GameObject pEffect;

    float timer = 0;
    float RTime = 0.1f;
    // Use this for initialization
    void Start () {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        TR = GetComponent<TrailRenderer>();
        Xrange = Screen.width / 2;
        TR.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        #region Stand Alone Input
        if (Input.GetMouseButtonDown(0))
        {
            startTouch = Input.mousePosition;
            if (startTouch.x > Xrange)
            {
                StartTouch();
            }



        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (timer < RTime)
            {
                if (startTouch.x > Xrange)
                    Instantiate(pEffect, rb.position, Quaternion.identity);

            }
                
            //Debug.Log(timer);
            timer = 0;
            StopTouch();
        }
        #endregion

        #region Mobile Input
        if (Input.touches.Length > Xrange)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                StartTouch();
                startTouch = Input.touches[0].position;

            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                Instantiate(pEffect, startTouch, Quaternion.identity);
                StopTouch();
            }
        }


        #endregion

        if(isDragging)
        {
            UpdateTouch();
        }

       
    }

    void UpdateTouch()
    {
        timer += Time.deltaTime;
        if (timer>RTime)
            TR.enabled = true;
        rb.position =cam.ScreenToWorldPoint(Input.mousePosition);
    }

    public void StartTouch()
    {
        isDragging = true;
    }

    void StopTouch()
    {
        isDragging = false;
        TR.enabled = false;
    }
}
