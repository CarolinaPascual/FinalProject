using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public float _moveSpeed;
    public bool _scrolling;

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
        if (_scrolling)
        {
            _moveVector.y = _moveSpeed;
            transform.Translate(_moveVector);
        }
    }
}
