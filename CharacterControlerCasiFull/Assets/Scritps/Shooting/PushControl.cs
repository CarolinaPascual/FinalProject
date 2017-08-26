using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushControl : MonoBehaviour {

    public float pushRange, pushCD, pushHeight,pushForce;
    Collider2D _collider;
    float pushCounter;
    PlayerControler _controller;
    private CVirtualJoystick _myVirtualJoystick;
	void Start () {
        pushCounter = pushCD;
        _collider = GetComponent<Collider2D>();
        _controller = GetComponent<PlayerControler>();
        _myVirtualJoystick = _controller.getVirtualJoystick();

    }
	
	// Update is called once per frame
	void Update () {
        pushCounter += Time.deltaTime;

        if (inputCheck())
            pushControl();
           

	}

    public void pushControl()
    {
        RaycastHit2D rayHit;
        for (int i =-1; i < 2; i++)
        {
            
            Vector3 rayOrigin = new Vector3(transform.position.x, transform.position.y + ((pushHeight / 2) * i));
            _collider.enabled = false;           
            
            rayHit = Physics2D.Raycast(rayOrigin, new Vector3(_controller.getFacingDirection(),0), pushRange);
            _collider.enabled = true;
            if (rayHit.collider !=null)                
            {
                //Debug.Log(rayHit.transform.gameObject.name);
                if (rayHit.transform.gameObject.tag == "Player")
                {
                    rayHit.transform.gameObject.GetComponent<PlayerControler>().startPush(_controller.getFacingDirection(),pushForce);
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
