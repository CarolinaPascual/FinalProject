using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour {

    public Animator _anim;
    public string _pressedState;
    public Material _hoverMaterial;
    public Material _normalMaterial;
    private MeshRenderer _meshRenderer;

	void Start () 
	{
        _meshRenderer = GetComponent<MeshRenderer>();
    }
		
	void Update () 
	{
		
	}
		
	void OnMouseEnter()
	{
        _meshRenderer.material.color = _hoverMaterial.color;
    }

	void OnMouseExit()
    {
        _meshRenderer.material.color = _normalMaterial.color;
        _anim.Play("None");
    }

	void OnMouseDown()
	{
        _anim.Play(_pressedState);
    }

	void OnMouseUp()
	{
        _anim.Play("None");
    }
}
