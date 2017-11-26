using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookControler : MonoBehaviour {

	public GameObject _model;
	[HideInInspector]
	public PlayerControler _owner;
	[HideInInspector]
	public PullgunController _controler;
	[HideInInspector]
	private Vector2 _rotDirection;
	private float rotSpeed = 20;
	private float rotation = 0;

	void Start () 
	{
		
	}
	

	void Update () 
	{
		rotateInTime ();
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerControler p = collision.gameObject.GetComponent<PlayerControler>();
		if (p != null) 
		{
			if (p != _owner)
			{
				_owner.setState(_owner.State_Normal);
				p.startPull ((_owner.transform.position - p.transform.position).normalized, _controler._pullSpeed);
				_controler._fired = false;
				destoyHook ();
				_controler.disableLine ();
			}
		} 
		else 
		{
			destoyHook ();
			_controler.disableLine ();
			_owner.setState (_owner.State_Normal);
			_controler._fired = false;
		}
	}

	public void destoyHook()
	{
		GameObject.Destroy (gameObject);
	}

	private void rotateInTime()
	{
		rotation = rotation + rotSpeed/100;
		_model.transform.Rotate (Vector3.up, rotation);
	}

	public void setRot(Vector2 rotVec)
	{
		_rotDirection = rotVec;
		if (_rotDirection.x == 1) 
		{
			if (_rotDirection.y == 0)
			{
				transform.Rotate (new Vector3 (0, 0, 90));
			}

			if (_rotDirection.y == 1)
			{
				transform.Rotate (new Vector3 (0, 0, 135));
			}

			if (_rotDirection.y == -1)
			{
				transform.Rotate (new Vector3 (0, 0, 45));
			}
		}

		if (_rotDirection.x == -1) 
		{
			if (_rotDirection.y == 0)
			{
				transform.Rotate (new Vector3 (0, 0, -90));
			}

			if (_rotDirection.y == 1)
			{
				transform.Rotate (new Vector3 (0, 0, -135));
			}

			if (_rotDirection.y == -1)
			{
				transform.Rotate (new Vector3 (0, 0, -45));
			}
		}
		if (_rotDirection.y == 1) 
		{
			if (_rotDirection.x == 0)
			{
				transform.Rotate (new Vector3 (0, 0, 180));
			}
		}
		if (_rotDirection.y == -1) 
		{
			if (_rotDirection.x == 0)
			{
				transform.Rotate (new Vector3 (0, 0, 0));
			}

		}
	}
}
