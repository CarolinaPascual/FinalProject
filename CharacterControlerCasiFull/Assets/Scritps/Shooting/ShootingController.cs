using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour {
	
    public GameObject _gunsBones;

    private GenericWeapon weaponScript;
	private GameObject equipedWeapon;
    private int facingDirection;
    private PlayerControler _controler;
    private CVirtualJoystick _virtualJoystick;
    private Vector2 input;

	void Start ()
    {
        _controler = GetComponent<PlayerControler>();
        _virtualJoystick = _controler.getVirtualJoystick();
        facingDirection = 1;
    }
	
	// Update is called once per frame
	void Update () {

		if (_virtualJoystick.getAtachedDevice () != null) 
		{
			input = _virtualJoystick.GetLeftStickClamped();
		}

		if (equipedWeapon != null)
		{
			rotateModel();
			equipedWeapon.transform.position = _gunsBones.transform.position;
		}
        

        if (input.x != 0)
        {
            facingDirection = (int)input.x;
        }

		if (_virtualJoystick.getAtachedDevice() != null)
		{
			if(_virtualJoystick.GetRightTriggerDown() || _virtualJoystick.GetRightTriggerPressed())
			{
				if (input == Vector2.zero)
				{
					input = new Vector2(facingDirection, 0);
				}
				if (_controler.getState() != _controler.State_Stuned)
				{
					if (equipedWeapon != null)
					{
						weaponScript.fire(input);
					}
				}
			}
		}
	}

    private void rotateModel()
    {
		if (_controler.getState() != _controler.State_Stuned && _controler.getState() != _controler.State_InputCursed)
		{
			if (_controler.isWallSlideing ()) 
			{
				if (_controler.getCharacterControler ()._collisionInfo.left) 
				{
					equipedWeapon.transform.eulerAngles = new Vector3(0, 180, 0);
				}
				if (_controler.getCharacterControler ()._collisionInfo.right) 
				{
					equipedWeapon.transform.eulerAngles = new Vector3(0, 0, 0);
				}
			}
			else
			{
				if (_virtualJoystick.getAtachedDevice() != null)
				{
					if (_virtualJoystick.GetLeftStickClamped().x > 0)
					{
						equipedWeapon.transform.eulerAngles = new Vector3(0, 0, 0);
					}
					if (_virtualJoystick.GetLeftStickClamped().x < 0)
					{
						equipedWeapon.transform.eulerAngles = new Vector3(0, 180, 0);
					}
				}
			}
		}  
		if (_controler.getState () == _controler.State_InputCursed) 
		{
			if (_controler.isWallSlideing ()) 
			{
				if (_controler.getCharacterControler ()._collisionInfo.left) 
				{
					equipedWeapon.transform.eulerAngles = new Vector3(0, 180, 0);
				}
				if (_controler.getCharacterControler ()._collisionInfo.right) 
				{
					equipedWeapon.transform.eulerAngles = new Vector3(0, 0, 0);
				}
			}
			else
			{
				if (_virtualJoystick.getAtachedDevice() != null)
				{
					if (_virtualJoystick.GetLeftStickClamped().x > 0)
					{
						equipedWeapon.transform.eulerAngles = new Vector3(0, 180, 0);
					}
					if (_virtualJoystick.GetLeftStickClamped().x < 0)
					{
						equipedWeapon.transform.eulerAngles = new Vector3(0, 0, 0);
					}
				}
			}
		}
    }

    public void equipWeapon(GameObject newWeapon)
    {
		if (equipedWeapon != null)
		{
			GameObject.Destroy(equipedWeapon.gameObject);
			equipedWeapon = null;
		}
        equipedWeapon = Instantiate(newWeapon, transform);        
        weaponScript = equipedWeapon.GetComponentInChildren<GenericWeapon>();
        weaponScript._owner = gameObject.GetComponent<PlayerControler>();
        equipedWeapon.transform.position = _gunsBones.transform.position;
    }

	public void clearWeapoon()
	{
		if (equipedWeapon != null)
		{
			GameObject.Destroy(equipedWeapon.gameObject);
			equipedWeapon= null;
		}
	}
}
