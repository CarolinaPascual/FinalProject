using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowgunControler : GenericWeapon {

	// Use this for initialization
	private float _durationCount;

	void Start () 
	{
		timeSinceLastShoot = shootCD;
		_durationCount = duration;
		_controler = _owner.GetComponent<ShootingController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		timeSinceLastShoot += Time.deltaTime;

		_durationCount -= Time.deltaTime;
		if (_durationCount <= 0)
		{
			_controler.clearWeapoon();
		}
	}

	public override void fire (Vector2 direction)
	{
		if (timeSinceLastShoot >= shootCD) 
		{
			GameObject auxBullet = Instantiate(bullet, _bulletSpawn.transform.position, Quaternion.identity);
			auxBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
			auxBullet.GetComponent<SlowControler> ()._owner = _owner;
			auxBullet.GetComponent<SlowControler> ()._isProyectile = true;
			timeSinceLastShoot = 0;
		}
	}

}
