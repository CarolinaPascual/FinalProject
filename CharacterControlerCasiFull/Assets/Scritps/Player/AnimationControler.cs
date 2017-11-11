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
	public string _stunedParameter;
	public string _aimDirectionXParameter;
	public string _aimDirectionYParameter;
	public string _hasGunParameter;
    public GameObject _model;
    public Animator _anim;

    private PlayerControler _controler;
	private CVirtualJoystick _virtualJoystick;
	private ShootingController _shootingControler;

	void Start ()
    {
        _controler = GetComponent<PlayerControler>();
		_virtualJoystick = _controler.getVirtualJoystick();
		_shootingControler = GetComponent<ShootingController> ();
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
		_anim.SetBool (_hasGunParameter, _shootingControler._hasGun);


		if (_shootingControler._hasGun) 
		{
			if (_virtualJoystick.getAtachedDevice() != null)
			{
				if (_virtualJoystick.GetLeftStickClamped().x == 0 && _virtualJoystick.GetLeftStickClamped().y == 0)
				{
					_anim.SetFloat (_aimDirectionYParameter, 0);
					_anim.SetFloat (_aimDirectionXParameter, 1);
				}
				else
				{
					_anim.SetFloat (_aimDirectionYParameter, _virtualJoystick.GetLeftStickClamped ().y);
					_anim.SetFloat (_aimDirectionXParameter, Mathf.Abs (_virtualJoystick.GetLeftStickClamped ().x));
				}
			}
		}

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
				if (_virtualJoystick.getAtachedDevice() != null)
				{
					_anim.SetFloat(_velocityXParameter, Mathf.Abs(_controler.getVelocityVector().x));
				}
            }
        }        
    }

    public void play(string aState, int aLayer)
    {
        _anim.Play(aState, aLayer);
    }

	private void rotateModel()
	{
		if (_controler.getState() != _controler.State_Stuned && _controler.getState() != _controler.State_InputCursed)
		{
			if (_controler.isWallSlideing ()) 
			{
				if (_controler.getCharacterControler ()._collisionInfo.right) 
				{
					_model.transform.eulerAngles = new Vector3(0, 90, 0);
				}
				if (_controler.getCharacterControler ()._collisionInfo.left) 
				{
					_model.transform.eulerAngles = new Vector3(0, -90, 0);
				}
			}
			else
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
		if (_controler.getState () == _controler.State_InputCursed) 
		{
			if (_controler.isWallSlideing ()) 
			{
				if (_controler.getCharacterControler ()._collisionInfo.right) 
				{
					_model.transform.eulerAngles = new Vector3(0, -90, 0);
				}
				if (_controler.getCharacterControler ()._collisionInfo.left) 
				{
					_model.transform.eulerAngles = new Vector3(0, 90, 0);
				}
			}
			else
			{
				if (_virtualJoystick.getAtachedDevice() != null)
				{
					if (_virtualJoystick.GetLeftStickClamped().x > 0)
					{
						_model.transform.eulerAngles = new Vector3(0, -90, 0);
					}
					if (_virtualJoystick.GetLeftStickClamped().x < 0)
					{
						_model.transform.eulerAngles = new Vector3(0, 90, 0);
					}
				}
			}
		}
	}
}
