using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour {
	
    public GameObject _gunsBones;

    private GenericWeapon weaponScript;
	private GameObject equipedWeapon;
	private GameObject _equipedWeaponModel;
    private int facingDirection;
	private PlayerControler _controler;
	[HideInInspector]
	public bool _hasGun = false;
    private CVirtualJoystick _virtualJoystick;
    private Vector2 input;
	private Vector3 _rotationVec;

	void Start ()
    {
        _controler = GetComponent<PlayerControler>();
        _virtualJoystick = _controler.getVirtualJoystick();
		_rotationVec = Vector3.zero;
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
			disapearWeapon();
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
				if (_controler.getState() != _controler.State_Stuned && !_controler.isWallSlideing ())
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
			if (_virtualJoystick.getAtachedDevice() != null)
			{
				if (_virtualJoystick.GetLeftStickClamped().x > 0)
				{
					_rotationVec.y = 0;
				}
				if (_virtualJoystick.GetLeftStickClamped().x < 0)
				{
					_rotationVec.y = 180;
				}
				if (_virtualJoystick.GetLeftStickClamped().y > 0)
				{
					if (_virtualJoystick.GetLeftStickClamped().x > 0)
					{
						_rotationVec.y = 0;
						_rotationVec.z = 45;
					}
					if (_virtualJoystick.GetLeftStickClamped().x < 0)
					{
						_rotationVec.y = 180;
						_rotationVec.z = 45;
					}
					if (_virtualJoystick.GetLeftStickClamped().x == 0)
					{
						_rotationVec.z = 90;
					}
				}
				if (_virtualJoystick.GetLeftStickClamped().y < 0)
				{
					if (_virtualJoystick.GetLeftStickClamped().x > 0)
					{
						_rotationVec.y = 0;
						_rotationVec.z = -45;
					}
					if (_virtualJoystick.GetLeftStickClamped().x < 0)
					{
						_rotationVec.y = 180;
						_rotationVec.z = -45;
					}
					if (_virtualJoystick.GetLeftStickClamped().x == 0)
					{
						_rotationVec.z = -90;
					}
				}
				if (Mathf.Abs(_virtualJoystick.GetLeftStickClamped().y) == 0)
				{
					_rotationVec.z = 0;
				}
				equipedWeapon.transform.eulerAngles = _rotationVec;//NO VUELVE A 0.
			}
		}  
		if (_controler.getState () == _controler.State_InputCursed) 
		{
			if (_virtualJoystick.getAtachedDevice() != null)
			{
				if (_virtualJoystick.GetLeftStickClamped().x > 0)
				{
					_rotationVec.y = 180;
					equipedWeapon.transform.eulerAngles = new Vector3(0, 180, 0);
				}
				if (_virtualJoystick.GetLeftStickClamped().x < 0)
				{
					_rotationVec.y = 0;
					equipedWeapon.transform.eulerAngles = new Vector3(0, 0, 0);
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
		_equipedWeaponModel = equipedWeapon.transform.Find("Model").gameObject;
		_hasGun = true;
    }

	public void disapearWeapon()
	{
		if ((_controler.getState() == _controler.State_Stuned) || (_controler.isWallSlideing()))
		{
			_equipedWeaponModel.SetActive(false);
			_hasGun = false;
		}
		else
		{
			_equipedWeaponModel.SetActive(true);		
			_hasGun = true;
		}
	}

	public void clearWeapoon()
	{
		if (equipedWeapon != null)
		{
			GameObject.Destroy(equipedWeapon.gameObject);
			equipedWeapon= null;
			_hasGun = false;
		}
	}
}