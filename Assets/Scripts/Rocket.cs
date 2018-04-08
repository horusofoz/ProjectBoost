using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    

    Rigidbody rigidBody;
    AudioSource myAudioSource;
    [SerializeField] float rcsThrust = 200f;
    [SerializeField] float mainThrust = 1000f;

    enum State { Alive, Dying, NextLevel };
    State state = State.Alive;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();   
	}
	
	// Update is called once per frame
	void Update ()
    {
         // TODO stop sound on death
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; } // Ignore collision

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly"); // TODO TBD
                break;
            case "Finish":
                state = State.NextLevel;
                Invoke("LoadNextScene", 1f); // Paramterise time
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstScene", 1f); // Paramterise time
                break;
        }
    }

    private void LoadNextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex < 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Allow for more than 2 levels
        }
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
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

        rigidBody.freezeRotation = true; // take manual control of rotation
        
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
