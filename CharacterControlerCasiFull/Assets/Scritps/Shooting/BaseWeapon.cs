using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : GenericWeapon {



	void Start () {
        timeSinceLastShoot = shootCD;
	}
	
	// Update is called once per frame
	void Update () {
        timeSinceLastShoot += Time.deltaTime;
	}

    public override void fire(Vector2 direction)
    {
       if(timeSinceLastShoot >= shootCD)
       {
            GameObject auxBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            auxBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            //Physics.IgnoreCollision(auxBullet.GetComponentInChildren<Collider>(), GetComponentInParent<Collider>());
            timeSinceLastShoot = 0;
       }

    }
}
