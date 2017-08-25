using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public float _moveSpeed;

    private Vector2 _moveVector;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        move();
	}

    private void move()
    {
        _moveVector.y = _moveSpeed;
        transform.Translate(_moveVector);   
    }
}
