using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControler : MonoBehaviour {

    public string _idle_walkState;
    public string _jumpState;
    public string _wallslideState;
    public string _velocityXParameter;
    public string _velocityYParameter;
    public string _airborneParameter;
    public string _wallSlideParameter;
    public string _stunedParameter;
    public GameObject _model;
    public Animator _anim;

    private PlayerControler _controler;
    private CVirtualJoystick _virtualJoystick;

	void Start ()
    {
        _controler = GetComponent<PlayerControler>();
        _virtualJoystick = _controler.getVirtualJoystick();
        _anim.Play(_idle_walkState);
        _model.transform.eulerAngles = new Vector3(0, 90, 0);

    }
	
	
	void Update ()
    {
        rotateModel();
        updateAnimations();
    }

    private void updateAnimations()
    {
        _anim.SetBool(_airborneParameter, !_controler.getCharacterControler()._collisionInfo.below);
        _anim.SetBool(_wallslideState, _controler.isWallSlideing());
        _anim.SetFloat(_velocityYParameter, _controler.getVelocityVector().y);
        if (_controler.getState() == _controler.State_Stuned)
        {
            _anim.SetBool(_stunedParameter, true);
        }
        else
        {
            _anim.SetBool(_stunedParameter, false);
        }
        
        if (_controler.getCharacterControler()._collisionInfo.below)
        {
            if (_controler.getCharacterControler()._collisionInfo.left || _controler.getCharacterControler()._collisionInfo.right)
            {
                
                if (_anim.GetFloat(_velocityXParameter) <= 0)
                {
                    _anim.SetFloat(_velocityXParameter, 0);
                }
                else
                {
                    _anim.SetFloat(_velocityXParameter, _anim.GetFloat(_velocityXParameter) - 0.5f);
                }
                
            }
            else
            {
                _anim.SetFloat(_velocityXParameter, Mathf.Abs(_controler.getVelocityVector().x));
            }
        }        
    }

    public void play(string aState, int aLayer)
    {
        _anim.Play(aState, aLayer);
    }

    private void rotateModel()
    {
		if (_virtualJoystick.getAtachedDevice() != null)
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
}
