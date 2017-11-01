using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public float _moveSpeed;
    public bool _scrolling;
    private float startingY;
    private float endY;
    private Vector2 _moveVector;

	void Start ()
    {
        startingY = transform.position.y;
        endY = transform.position.y + LevelManager.Inst._levelSpawner.levelHeight;
	}
	
	void Update ()
    {
        move();
        if(transform.position.y>= endY)
        {
            _scrolling = false;
        }
	}

    private void move()
    {
        if (_scrolling)
        {
            _moveVector.y = _moveSpeed/100;
            transform.Translate(_moveVector);
        }
    }
}
