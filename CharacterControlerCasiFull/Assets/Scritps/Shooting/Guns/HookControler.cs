using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookControler : MonoBehaviour {

	[HideInInspector]
	public GameObject _owner;
	[HideInInspector]
	public PullgunController _controler;

	void Start () 
	{
		
	}
	

	void Update () 
	{
		
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerControler p = collision.gameObject.GetComponent<PlayerControler>();
		if (p != null) 
		{
			_owner.GetComponent<PlayerControler>().setState(_owner.GetComponent<PlayerControler>().State_Normal);
			p.startPull ((_owner.transform.position - p.transform.position).normalized, _controler._pullSpeed);
			_controler._fired = false;
			destoyHook ();
		} 
		else 
		{
			destoyHook ();
			_owner.GetComponent<PlayerControler> ().setState (_owner.GetComponent<PlayerControler> ().State_Normal);
			_controler._fired = false;
		}
	}

	public void destoyHook()
	{
		GameObject.Destroy (gameObject);
	}
}
