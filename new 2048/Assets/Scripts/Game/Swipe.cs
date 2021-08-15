using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    private bool isDraging = false;
    private Vector2 startTouch, swipeDelta;

    public bool Tap { get { return tap; } }
    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }

    private void Update()
    {
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;

        #region Keyboard Input
        if ((!UIManager.Instance.SettingView.activeSelf) && (!UIManager.Instance.Menu.activeSelf) && (!UIManager.Instance.GameOver.activeSelf))
        {
            if (Input.GetKey("left"))
            {
                swipeLeft = true;
            }
            else if (Input.GetKey("right"))
            {
                swipeRight = true;
            }
            else if (Input.GetKey("down"))
            {
                swipeDown = true;
            }
            else if (Input.GetKey("up"))
            {
                swipeUp = true;
            }
        }
        #endregion

        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0) && (!UIManager.Instance.SettingView.activeSelf) && (!UIManager.Instance.Menu.activeSelf) && (!UIManager.Instance.GameOver.activeSelf))
        {
            tap = true;
            isDraging = true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDraging = false;
            Reset();
        }
        #endregion

        #region Mobile Inputs
        if ((Input.touches.Length > 0) && (!UIManager.Instance.SettingView.activeSelf) && (!UIManager.Instance.Menu.activeSelf) && (!UIManager.Instance.GameOver.activeSelf))
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                isDraging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDraging = false;
                Reset();
            }
        }
        #endregion
    
        // Calcola la distanza
        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touches.Length > 0)
                swipeDelta = Input.touches[0].position - startTouch;
            else 
            if (Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
        }

        // Abbiamo raggiunto la distanza minima ?
        if(swipeDelta.magnitude > ControlManager.Instance.Magnitude)
        {
            GC.Collect();
            // Quale direzione?
            float vector_x = swipeDelta.x;
            float vector_y = swipeDelta.y;


            // camera obliqua
            // Sinistra o destra ?
            /*if ((vector_x < 0) &&(vector_y < 0))
                swipeLeft = true;
            else if((vector_x > 0) && (vector_y > 0))
                swipeRight = true;
            
            // Su o giù
            else if ((vector_x > 0) && (vector_y < 0))
                swipeDown = true;
            else if ((vector_x < 0) && (vector_y > 0))            
                swipeUp = true;
                */

            // camera dritta
            if (Mathf.Abs(vector_x) > Mathf.Abs(vector_y))
            {
                // Sinistra o destra ?
                if (vector_x < 0)
                    swipeLeft = true;
                else
                    swipeRight = true;
            }
            else
            {
                // Su o giù
                if (vector_y < 0)
                    swipeDown = true;
                else
                    swipeUp = true;
            }

            Reset();
        }
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }
}