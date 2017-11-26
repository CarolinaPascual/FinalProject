using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunControler : MonoBehaviour {

    public float _stunTime = 2;
    public Collider2D killerObject;//To ignore colition.
	public Collider2D endGameCollider;//To ignore colition.
    public Collider2D myCollider;//To ignore colition.
	[HideInInspector]
	public PlayerControler _owner;

    void Start ()
    {
		Physics2D.IgnoreCollision(myCollider, killerObject, true);
		Physics2D.IgnoreCollision(myCollider, endGameCollider, true);
    }

    void Update ()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerControler p = collision.gameObject.GetComponent<PlayerControler>();
        if (p != null)
        {
			if (_owner != null)
			{
				if (p != _owner) 
				{
					p.setState(p.State_Stuned, _stunTime);
				}
			}
			else
			{
				p.setState(p.State_Stuned, _stunTime);
			}
        }
    }
}
