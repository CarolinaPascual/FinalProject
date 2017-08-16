using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericWeapon : MonoBehaviour {

    public GameObject bullet;
    public int dmg;
    public float shootCD;
    public float duration;  
   protected float timeSinceLastShoot;
    public float bulletSpeed;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public abstract void fire(Vector2 direction);
    

  

}
