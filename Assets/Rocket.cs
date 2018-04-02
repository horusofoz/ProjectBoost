﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {




	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        ProcessInput();
	}

    private void ProcessInput()
    {
        // Thrust
        if(Input.GetKey(KeyCode.Space)) // Can thrust while rotating
        {
            print("Thrusting");
        }

        //Rotate
        if (Input.GetKey(KeyCode.A))
        {
            print("Rotating left");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            print("Roating right");
        }
    }
}
