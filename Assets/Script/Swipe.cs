using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour {
    private bool touch, swipeLeft, swipeRight, swipeUp, swipeDown,tap;
    private bool isDragging = false;
    private Vector2 startTouch, swipeDelta;
    float Xrange = 0;
    float timer = 0;
    float RTime = 0.1f;

    private void Start()
    {
        Xrange = Screen.width / 2;
    }

    // Update is called once per frame
    void FixedUpdate () {
        tap = touch = swipeLeft = swipeUp = swipeRight = swipeDown = false;
        
        #region Stand Alone Input
        if (Input.GetMouseButtonDown(0))
        {
            startTouch = Input.mousePosition;
            if (startTouch.x > Xrange)
            {
                touch = true;
                isDragging = true;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (timer < RTime)
            {
                if (startTouch.x > Xrange)
                    tap = true;
            }
            //Debug.Log(timer);
            timer = 0;

            Reset();
        }
        #endregion

        #region Mobile Input
        if (Input.touches.Length > Xrange)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                touch = true;
                isDragging = true;
                startTouch = Input.touches[0].position;

            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
                Reset();
        }


        #endregion

        swipeDelta = Vector2.zero;
        if(isDragging)
        {
            timer += Time.deltaTime;
            if (Input.touches.Length > 0)
                swipeDelta = Input.touches[0].position - startTouch;
            else if (Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
        }

        if (swipeDelta.magnitude > 125)
        {
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                if (x > 0)
                    swipeRight = true;
                else
                    swipeLeft = true;
            }
            else
            {
                if (y > 0)
                    swipeUp = true;
                else
                    swipeDown = true;
            }
            Reset();
        }
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDragging = false;
    }
    

    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public Vector2 StartTouch { get { return startTouch; } }
    public bool Touch { get { return touch; } }
    public bool Tap { get { return tap; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }
}
