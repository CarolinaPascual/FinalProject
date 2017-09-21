using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullControler : MonoBehaviour {

    public PullgunControler _controler;
    [HideInInspector]
    public PlayerControler _owner;

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
            if (p != _owner)
            {
                _controler.hit(p);
            }
        }
    }

    public void destroyHook()
    {
        GameObject.Destroy(gameObject);
    }

}
