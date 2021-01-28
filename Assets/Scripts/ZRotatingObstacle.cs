using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZRotatingObstacle : MonoBehaviour
{
    private const int MaxRotation = 360;

    [SerializeField] int _rotationFactor = 100;

    private const int MinRotation = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        if (transform.rotation.z >= MaxRotation)
            transform.Rotate(0, 0, 0);
        transform.Rotate(Vector3.forward * _rotationFactor * Time.deltaTime);
    }
}
