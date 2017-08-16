using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour {

    public GameObject equipedWeapon;
    private GenericWeapon weaponScript;
    int facingDirection;
	void Start () {

        facingDirection = 1;
        equipWeapon(equipedWeapon);
    }
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.A))
            facingDirection = -1;
        if (Input.GetKeyDown(KeyCode.D))
            facingDirection = 1;

        if(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftControl))
        {
            Vector2 _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
           
            if (_input == Vector2.zero)
            {
                _input = new Vector2(facingDirection, 0);
            }
            
            weaponScript.fire(_input);
        }
	}

    void equipWeapon(GameObject newWeapon)
    {
        //GameObject.Destroy(equipedWeapon.gameObject);
        equipedWeapon = Instantiate(newWeapon, transform);        
        weaponScript = equipedWeapon.GetComponentInChildren<GenericWeapon>();
    }
}
