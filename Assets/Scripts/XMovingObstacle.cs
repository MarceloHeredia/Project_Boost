using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XMovingObstacle : MonoBehaviour
{
    private enum Direction { Right, Left }; // to Control which direction the obstacle will move

    [SerializeField] int _minX = 20;
    [SerializeField] int _maxX = 44;
    [SerializeField]  float _moveFactor = 15f;
    private Direction _currentDirection = Direction.Right;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ValidateObstaclePosition();
        MoveObstacle();
    }
    
    private void MoveObstacle()
    {
        switch (_currentDirection)
        {
            case Direction.Right:
                transform.Translate(Vector3.right * _moveFactor * Time.deltaTime);
                break;
            case Direction.Left:
                transform.Translate(Vector3.left * _moveFactor * Time.deltaTime);
                break;
        }
    }

    public void ValidateObstaclePosition()
    {
        switch (_currentDirection)
        {
            case Direction.Right when transform.position.x >= _maxX:
                _currentDirection = Direction.Left;
                break;
            case Direction.Left when transform.position.x <= _minX:
                _currentDirection = Direction.Right;
                break;
            default:
                break;
        }
    }
}
