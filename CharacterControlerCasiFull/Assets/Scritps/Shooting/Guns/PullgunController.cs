using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullgunController : GenericWeapon {

	[HideInInspector]
	public bool _fired;
	public float _pullSpeed;
	public float _maxLenght;
	public LineRenderer _lineRender;
	private float _durationCount;
	private GameObject _lastHook;
	private Vector3 _distToHook;

	void Start () 
	{
		_controler = _owner.GetComponent<ShootingController>();
		timeSinceLastShoot = shootCD;
		_durationCount = duration;
	}

	void Update () 
	{
		timeSinceLastShoot += Time.deltaTime;
		_durationCount -= Time.deltaTime;
		if (_durationCount <= 0)
		{
			_controler.clearWeapoon();
			_owner.setState (_owner.State_Normal);
			if (_lastHook != null) 
			{
				_lastHook.GetComponent<HookControler> ().destoyHook ();
			}
		}
		hookLenghtCheck();
		updateLine ();
	}

	private void hookLenghtCheck()
	{
		if (_lastHook != null)
		{
			_distToHook = _lastHook.transform.position - _owner.transform.position;
			if (_distToHook.magnitude >= _maxLenght)
			{
				_lastHook.GetComponent<HookControler>().destoyHook();
				_owner.setState (_owner.State_Normal);
				_fired = false;
				_lineRender.enabled = false;
			}
		}
	}


	public override void fire(Vector2 direction)
	{
		if (!_fired) 
		{
			if (timeSinceLastShoot >= shootCD) 
			{
				GameObject auxBullet = Instantiate (bullet, _bulletSpawn.transform.position, Quaternion.identity);
				auxBullet.GetComponent<Rigidbody2D> ().velocity = direction * bulletSpeed;
				auxBullet.GetComponent<HookControler> ()._controler = this;
				_lastHook = auxBullet;
				_lastHook.GetComponent<HookControler> ()._owner = _owner;
				_lastHook.GetComponent<HookControler> ().setRot (direction);
				timeSinceLastShoot = 0;
				_fired = true;
				_owner.setState(_owner.State_Pulling);
				renderLine ();
			}
		}
		else 
		{
			if (_owner.getVirtualJoystick ().GetRightTriggerDown ()) 
			{
				_owner.setState (_owner.State_Normal);
   				_lastHook.GetComponent<HookControler> ().destoyHook ();
				_fired = false;
				_lineRender.enabled = false;
			}
		}
	}

	private void renderLine()
	{
		_lineRender.enabled = true;
	}

	public void disableLine()
	{
		_lineRender.enabled = false;
	}

	private void updateLine()
	{ 
		if (_lineRender.enabled)
		{
			_lineRender.SetPosition (0, _bulletSpawn.transform.position);
			if (_lastHook != null)
			{

				_lineRender.SetPosition (1, _lastHook.transform.position);
			}
		}
	}
}
