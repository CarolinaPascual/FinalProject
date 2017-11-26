using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour {

	public Material hoverMat;
	private Vector3 _moveVector;
	private Material origianlMat;
	private MeshRenderer meshR;
	private bool _clicked = false;
	void Start () 
	{
		meshR = GetComponent<MeshRenderer> ();
		origianlMat = meshR.material;
	}
		
	void Update () 
	{
		
	}
		
	void OnMouseEnter()
	{
		meshR.material = hoverMat;
	}

	void OnMouseExit()
	{
		meshR.material = origianlMat;
	}

	void OnMouseDown()
	{
		_clicked = true;
		_moveVector.z =- 0.5f;
		transform.Translate (_moveVector);
	}

	void OnMouseUp()
	{
		if (_clicked)
		{
			_clicked = false;
			_moveVector.z =+ 0.5f;
			transform.Translate (_moveVector);
		}
	}
}
