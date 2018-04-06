using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource myAudioSource;
    bool isThrusting;
    [SerializeField] float rcsThrust = 200f;
    [SerializeField] float mainThrust = 1000f;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();   
	}
	
	// Update is called once per frame
	void Update ()
    {
        Thrust();
        Rotate();
	}

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly"); // TODO TBD
                break;
            case "Fuel":
                Debug.Log("Fuel"); // TODO Rocket refueled
                break;
            default:
                Debug.Log("Dead"); // TODO Rocket dies
                break;
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space)) // Can thrust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up * (mainThrust * Time.deltaTime));
            if (!myAudioSource.isPlaying)
            {
                myAudioSource.Play();
            }
        }
        else
        {
            myAudioSource.Stop();
        }
    }

    private void Rotate()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            rigidBody.freezeRotation = true; // take manual control of rotation
        }
        
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // Resume physics control of rotation
    }

    
}
