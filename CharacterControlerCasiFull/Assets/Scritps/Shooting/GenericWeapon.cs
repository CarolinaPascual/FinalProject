﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericWeapon : MonoBehaviour {

    public GameObject bullet;
    [HideInInspector]
    public PlayerControler _owner;
	[HideInInspector]
	public ShootingController _controler;
	public GameObject _bulletSpawn;
    public float shootCD;
    public float duration;
    protected float timeSinceLastShoot;
    public float bulletSpeed;

	void Start ()
    {
		
	}

    void Update()
    {
        
    }

    public abstract void fire(Vector2 direction);

}
