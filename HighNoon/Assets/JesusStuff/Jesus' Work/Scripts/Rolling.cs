using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rolling : MonoBehaviour
{
    //private float rollForce = 10000f;

    //private GameObject playr;

    private void Awake()
    {
        //playr = GameObject.Find("Player");
    }

    void FixedUpdate ()
    {

        if (Input.GetAxis("Left Stick Horizontal") == 1 && Input.GetButtonDown("Left Bumper") == true)
        {

        }
        if (Input.GetAxis("Left Stick Horizontal") == -1)
        {

        }
        if (Input.GetAxis("Right Stick Horizontal") == 1)
        {

        }
        if (Input.GetAxis("Right Stick Horizontal") == -1)
        {

        }
    }
}