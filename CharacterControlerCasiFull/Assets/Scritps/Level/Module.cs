using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour {

    private float height;
	void Awake () {
        height = GetComponent<BoxCollider>().size.y;
	}
	
	public float getHeight()
    {
        return height;
    }
}
