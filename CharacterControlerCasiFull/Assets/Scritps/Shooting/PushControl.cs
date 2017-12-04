using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushControl : MonoBehaviour {

    public float pushRange, pushCD, pushHeight, pushForce;
    public ParticleSystem _particleSystem;
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

    public void pushBehavior()
    {
        pushCounter += Time.deltaTime;

		if (_myVirtualJoystick.getAtachedDevice() != null)
		{
			if (_myVirtualJoystick.GetLeftStickClamped().x != 0)
			{
				facingDirection = (int)_myVirtualJoystick.GetLeftStickClamped().x;
			}
		}
        if (inputCheck())
        {
            pushControl();
            _animControl.play("Push", 1);
            _particleSystem.Emit(5);
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
			if (_myVirtualJoystick.getAtachedDevice() != null)
			{
				if (_myVirtualJoystick.GetAction3Down())
				{
					pushCounter = 0;
					return true;    
				}                  
			}
        }
        return false; 
    }
}
