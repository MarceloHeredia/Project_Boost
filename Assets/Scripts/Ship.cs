using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ship : MonoBehaviour
{

    #region Variables

    [SerializeField] float _rotationThrust = 100f;
    [SerializeField] float _mainThrust = 100f;
    [SerializeField] float _loadingTime = 0.5f;

    [SerializeField] private AudioClip _mainEngineSound;
    [SerializeField] private AudioClip _successSound;
    [SerializeField] private AudioClip _deathSound;

    private Rigidbody _rigidBody;
    private AudioSource _audioSource;

    enum State {Alive, Dying, Transcending}

    private State _state = State.Alive;
    #endregion

    #region Initialization and Frames
    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //todo stop sound upon death
        if (_state == State.Alive)
        {
            RespondToThrustInput();
            RespondeToRotateInput();
        }
    }

    protected void LateUpdate() //used to lock X and Y rotations
    {
        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_state != State.Alive) { return; } //ignore collisions when not alive
        switch (collision.gameObject.tag)
        {
            case "Friendly":
            case "Respawn":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }
    #endregion


    #region Input Responses
    /// <summary> Rotate the rocket based on user inputs</summary>
    private void RespondeToRotateInput()
    {
        //_rigidBody.freezeRotation = true; //take manual control of rotation
        _rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

        float frameRotation = _rotationThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * frameRotation);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * frameRotation);
        }

        //_rigidBody.freezeRotation = false; // resume physics control of rotation
        _rigidBody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
    }

    /// <summary> Method responsible for the rocket's thrust and it's sounds</summary>
    private void RespondToThrustInput()
    {
        //float frameThrust = _mainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space)) //can thrust while rotating
        {
            ApplyThrust();
        }
        else
        {
            _audioSource.Stop();
        }
    }

    private void ApplyThrust()
    {
        _rigidBody.AddRelativeForce(Vector3.up * _mainThrust);
        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(_mainEngineSound);
        }
    }
    #endregion

    #region Changing Scene
    private void StartDeathSequence()
    {
        _state = State.Dying;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_deathSound);
        Invoke(nameof(LoadFirstLevel), _loadingTime);
    }

    private void StartSuccessSequence()
    {
        _state = State.Transcending;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_successSound);
        Invoke(nameof(LoadNextLevel), _loadingTime); //parameterise time
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(1); //todo allow for more than 2 levels
    }

    void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
    #endregion

}
