using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    [SerializeField] float _rcsThrust = 100f;
    [SerializeField] float _mainThrust = 100f;

    private Rigidbody _rigidBody;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Thrust();
        Rotate();
    }


    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                print("OK"); //todo remove
                break;
            case "Fuel":
                print("Fuel");//todo remove
                break;
            default:
                print("Dead");
                //todo kill player
                break;
        }
    }
    /// <summary> Rotate the rocket based on user inputs</summary>
    private void Rotate()
    {
        //_rigidBody.freezeRotation = true; //take manual control of rotation
        _rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

        float frameRotation = _rcsThrust * Time.deltaTime;
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
    private void Thrust()
    {
        //float frameThrust = _mainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space)) //can thrust while rotating
        {
            _rigidBody.AddRelativeForce(Vector3.up * _mainThrust);
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
        else
        {
            _audioSource.Stop();
        }
    }
}
