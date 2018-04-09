using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{  
    Rigidbody rigidBody;
    AudioSource myAudioSource;

    [SerializeField] float rcsThrust = 200f;
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip levelComplete;
    [SerializeField] AudioClip deathExplosion;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

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
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
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
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.NextLevel;
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(levelComplete);
        successParticles.Play();
        Invoke("LoadNextScene", levelLoadDelay); // Paramterise time
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(deathExplosion);
        deathParticles.Play();
        Invoke("LoadSameScene", levelLoadDelay); // Paramterise time
    }

    private void LoadNextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex < 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Allow for more than 2 levels
        }
    }

    private void LoadSameScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space)) // Can thrust while rotating
        {
            ApplyThrust();
        }
        else
        {
            myAudioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void RespondToRotateInput()
    {

        rigidBody.freezeRotation = true; // take manual control of rotation
        
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // Resume physics control of rotation
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * (mainThrust * Time.deltaTime));
        if (!myAudioSource.isPlaying)
        {
            myAudioSource.PlayOneShot(mainEngine);

        }
        if (!mainEngineParticles.isPlaying)
        {
            mainEngineParticles.Play();
        }
    }
}
