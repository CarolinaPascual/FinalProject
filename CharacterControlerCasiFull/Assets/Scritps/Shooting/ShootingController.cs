using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour {

    public GameObject equipedWeapon;
    private GenericWeapon weaponScript;
    private int facingDirection;
    private PlayerControler _controler;
    private CVirtualJoystick _virtualJoystick;
    private Vector2 input;
	void Start ()
    {
        _controler = GetComponent<PlayerControler>();
        _virtualJoystick = _controler.getVirtualJoystick();
        facingDirection = 1;
        equipWeapon(equipedWeapon);
    }
	
	// Update is called once per frame
	void Update () {

        
        input = _virtualJoystick.GetLeftStickClamped();

        if (input.x != 0)
        {
            facingDirection = (int)input.x;
        }

        if(_virtualJoystick.GetRightTriggerDown() || _virtualJoystick.GetRightTriggerPressed())
        {
            if (input == Vector2.zero)
            {
                input = new Vector2(facingDirection, 0);
            }
            
            weaponScript.fire(input);
        }
	}

    void equipWeapon(GameObject newWeapon)
    {
        //GameObject.Destroy(equipedWeapon.gameObject);
        equipedWeapon = Instantiate(newWeapon, transform);        
        weaponScript = equipedWeapon.GetComponentInChildren<GenericWeapon>();
    }
}
