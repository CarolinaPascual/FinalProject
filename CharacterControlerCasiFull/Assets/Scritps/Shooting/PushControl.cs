﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushControl : MonoBehaviour {

    public float pushRange, pushCD, pushHeight, pushForce;
    private float pushCounter;
    private int facingDirection = 1;
    private PlayerControler _controller;
    private AnimationControler _animControl;
    private Collider2D _collider;
    private CVirtualJoystick _myVirtualJoystick;

	void Start () {
        pushCounter = pushCD;
        _collider = GetComponent<Collider2D>();
        _controller = GetComponent<PlayerControler>();
        _animControl = GetComponent<AnimationControler>();
        _myVirtualJoystick = _controller.getVirtualJoystick();

    }
	
	// Update is called once per frame
	void Update () {
        pushCounter += Time.deltaTime;

        if (_myVirtualJoystick.GetLeftStickClamped().x != 0)
        {
            facingDirection = (int)_myVirtualJoystick.GetLeftStickClamped().x;
        }

        if (inputCheck())
        {
            if(_controller.getState() != _controller.State_Stuned)
            {
                pushControl();
                _animControl.play("Push", 1);
            }
        }
	}

    public void pushBehavior()
    {
        pushCounter += Time.deltaTime;

        if (_myVirtualJoystick.GetLeftStickClamped().x != 0)
        {
            facingDirection = (int)_myVirtualJoystick.GetLeftStickClamped().x;
        }

        if (inputCheck())
        {
            pushControl();
            _animControl.play("Push", 1);
        }
    }

    public void pushControl()
    {
        RaycastHit2D rayHit;
        for (int i =-1; i < 2; i++)
        {
            
            Vector3 rayOrigin = new Vector3(transform.position.x, transform.position.y + ((pushHeight / 2) * i));
            _collider.enabled = false;           
            
            rayHit = Physics2D.Raycast(rayOrigin, new Vector3(facingDirection, 0), pushRange);
            _collider.enabled = true;
            if (rayHit.collider !=null)                
            {
                if (rayHit.transform.gameObject.tag == "Player")
                {
                    rayHit.transform.gameObject.GetComponent<PlayerControler>().startPush(facingDirection, pushForce);
                }
            }
        }
    }

    public bool inputCheck()
    {
        if (pushCounter >= pushCD)
        {
            if (_myVirtualJoystick.GetLeftTriggerDown())
            {
                pushCounter = 0;
                return true;    
            }                  
        }
        return false;
           
    }

}
