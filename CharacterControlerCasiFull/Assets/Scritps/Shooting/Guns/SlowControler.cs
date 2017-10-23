using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowControler : MonoBehaviour {

	[HideInInspector]
	public float _slowTime = 2;
	[HideInInspector]
	public PlayerControler _owner;
	[HideInInspector]
	public bool _isProyectile;

	void Start ()
	{
		Destroy (gameObject, 10);
	}

	void Update ()
	{

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerControler p = collision.gameObject.GetComponent<PlayerControler>();
		if (p != null)
		{
			if (p != _owner) 
			{
				p.setState(p.State_Slowed, _slowTime);
				if (_isProyectile)
				{
					Destroy (gameObject);
				}
			}
		}
	}
}
