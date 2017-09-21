using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunControler : MonoBehaviour {

    public float _stunTime = 2;

	void Start ()
    {
		
	}

	void Update ()
    {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerControler p = collision.gameObject.GetComponent<PlayerControler>();
        if (p != null)
        {
            p.setState(p.State_Stuned, _stunTime);
        }
    }
}
