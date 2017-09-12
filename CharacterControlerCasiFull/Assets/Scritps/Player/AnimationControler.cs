using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControler : MonoBehaviour {

    public GameObject _model;
    public Animator _anim;

    private PlayerControler _controler;
    private CVirtualJoystick _virtualJoystick;

	void Start ()
    {
        _controler = GetComponent<PlayerControler>();
        _virtualJoystick = _controler.getVirtualJoystick();
        _anim.Play("Idle-Walk");
        _model.transform.eulerAngles = new Vector3(0, 90, 0);

    }
	
	
	void Update ()
    {
        rotateModel();
        updateAnimations();
    }

    private void updateAnimations()
    {        
        if (_controler.getCharacterControler()._collisionInfo.below)
        {
            _anim.Play("Idle-Walk");
            _anim.SetFloat("VelocityX", Mathf.Abs(_controler.getVelocityVector().x));
        }
        else
        {
            _anim.Play("Jump");
        }
    }

    public void play(string aState, int aLayer)
    {
        _anim.Play(aState, aLayer);
    }

    private void rotateModel()
    {
        if (_virtualJoystick.GetLeftStickClamped().x > 0)
        {
            _model.transform.eulerAngles = new Vector3(0, 90, 0);
        }
        if (_virtualJoystick.GetLeftStickClamped().x < 0)
        {
            _model.transform.eulerAngles = new Vector3(0, -90, 0);
        }
    }
}
