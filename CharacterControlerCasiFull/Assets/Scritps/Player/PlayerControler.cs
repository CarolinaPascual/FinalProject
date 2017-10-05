using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterControler2D))]
public class PlayerControler : MonoBehaviour {

    #region Publics
    [Header("Movement Variables")]
    public float _moveSpeed = 6;
    public float _jumpHeight = 4;
    public float _timeToJumpApex = .4f;
    public float _accelerationTimeAirborned = .2f;
    public float _accelerationTimeGrounded = .1f;

    [Header("Wallslide Variables")]
    public float _wallSlideSpeedMax = 3;
    public float _wallStickTime = .25f;

    [Header("Dash Variables")]
    public float _dashForce = 1;
    public float _dashCooldown = 1;

    [Header("Walljump Variables")]
    public Vector2 _wallJumpClimb;
    public Vector2 _wallJumpOff;
    public Vector2 _wallJumpLeap;

    [Header("States Variables")]
    public int _numberOfIFrames;

    #endregion

    #region States
    private int _CurrentState;
    [HideInInspector]
    public int State_Normal = 0;
    [HideInInspector]
    public int State_Stuned = 1;
    [HideInInspector]
    public int State_Dead = 2;
    [HideInInspector]
    public int State_Respawning = 3;

    private float _currentStateTime = 0;
    private float _currentStateFrames = 0;
    private float _stunedTime;
    #endregion 

    #region Privates
    private bool _wallSliding = false;
    private bool _dashed = false;
    private bool _isPushed = false;
    private bool _isPulled = false;
    private int _pushDirection;
    private float _dashCooldownCount;
    private float _pushForce;
    private float _pullForce;
    private float _gravity;
    private float _jumpVelocity;
    private float _timeToWallUnstick;
    private float _velocityXDamper;
    private Vector3 _velocity;
    private Vector2 _input;
    private Vector2 _pullDirection;
    private CharacterControler2D _controller;
    private CVirtualJoystick _myVirtualJoystick;
    private PushControl _pushControl;
    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        _myVirtualJoystick = new CVirtualJoystick();
    }

    void Start()
    {
        _controller = GetComponent<CharacterControler2D>();
        _gravity = -(2 * _jumpHeight) / Mathf.Pow(_timeToJumpApex, 2);
        _jumpVelocity = Mathf.Abs(_gravity) * _timeToJumpApex;
        _myVirtualJoystick.init();
        _pushControl = GetComponent<PushControl>();
        setState(State_Normal);
    }

    void Update()
    {
        stateNormal();
        stateStuned();
        if (_controller._collisionInfo.above || _controller._collisionInfo.below)
        {
            _velocity.y = 0;
        }
    }

    #endregion

    #region Movement

    private void wallSlide()
    {
        int wallDirx = (_controller._collisionInfo.left) ? -1 : 1;
        _wallSliding = false;
        if ((_controller._collisionInfo.left || _controller._collisionInfo.right) && !_controller._collisionInfo.below && _velocity.y < 0)
        {
            _wallSliding = true;
            if (_velocity.y < -_wallSlideSpeedMax)
            {
                _velocity.y = -_wallSlideSpeedMax;
            }

            if (_timeToWallUnstick > 0)
            {
                _velocityXDamper = 0;
                _velocity.x = 0;
                if (_input.x != wallDirx && _input.x != 0)
                {
                    _timeToWallUnstick -= Time.deltaTime;
                }
            }
            else
            {
                _timeToWallUnstick = _wallStickTime;
            }
        }
    }

    private void dash()
    {
        if (_myVirtualJoystick.GetAction3Down() && !_dashed)
        {
            if (_input.x == 0)
            {
                _velocity.x = _dashForce * _controller._collisionInfo.faceDir;
                _dashed = true;
                return;
            }
            else
            {
                _velocity.x = _dashForce * _input.x;
                _dashed = true;
                return;
            }

        }
        if (_dashed)
        {
            _dashCooldownCount += Time.deltaTime;
            if (_dashCooldownCount >= _dashCooldown)
            {
                _dashCooldownCount = 0;
                _dashed = false;
            }
        }
    }

    private void jump()
    {
        int wallDirx = (_controller._collisionInfo.left) ? -1 : 1;
        if (_myVirtualJoystick.GetAction1Down())
        {
            if (_wallSliding)
            {
                if (wallDirx == _input.x)
                {
                    _velocity.x = -wallDirx * _wallJumpClimb.x;
                    _velocity.y = _wallJumpClimb.y;
                }
                else if (_input.x == 0)
                {
                    _velocity.x = -wallDirx * _wallJumpOff.x;
                    _velocity.y = _wallJumpOff.y;
                }
                else
                {
                    _velocity.x = -wallDirx * _wallJumpLeap.x;
                    _velocity.y = _wallJumpLeap.y;
                }
            }
            if (_controller._collisionInfo.below)
            {
                if (_input.y < 0)
                {
                    _controller._ignoreOneWayPlatformsThisFrame = true;
                }
                else
                {
                    _velocity.y = _jumpVelocity;
                }
            }
        }
    }

    private void movement()
    {
        float targetVelocityX = _input.x * _moveSpeed;
        _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXDamper, (_controller._collisionInfo.below) ? _accelerationTimeGrounded : _accelerationTimeAirborned);
    }

   

    #endregion

    #region States

    public void setState(int aState, float aTimeInState = 0)
    {
        if (aState == State_Stuned)
        {
            if (_CurrentState != State_Stuned)
            {
                _stunedTime = aTimeInState;
                _CurrentState = aState;
                _currentStateTime = 0;
                _currentStateFrames = 0;
                _input.x = 0;
                _velocity.x = 0;
            }
        }
        if (aState == State_Normal)
        {
            _CurrentState = aState;
            _currentStateTime = 0;
            _currentStateFrames = 0;
        }

        if(aState == State_Dead)
        {
            _CurrentState = aState;
            _currentStateTime = 0;
            _currentStateFrames = 0;
            _input.x = 0;
            _velocity.x = 0;
        }
    }

    public int getState()
    {
        return _CurrentState;
    }

    public float getCurrentStateTime(bool inFrames)
    {
        if (inFrames)
        {
            return _currentStateFrames;
        }
        else if (!inFrames)
        {
            return _currentStateTime;
        }
        return 0;
    }

    private void stateNormal()
    {
        if (getState() == State_Normal)
        {
            catchInput();
            movement();
            jump();
            wallSlide();
            push();
            pushControl();
            pull();
            dash();
            _velocity.y += _gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
            _currentStateFrames += 1;
            _currentStateTime += Time.deltaTime;
        }
    }

    private void stateStuned()
    {
        if (getState() == State_Stuned)
        {
            movement();            
            push();
            pull();
            _velocity.y += _gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
            _currentStateFrames += 1;
            _currentStateTime += Time.deltaTime;
            if (_currentStateTime >= _stunedTime)
            {
                setState(State_Normal);
                _stunedTime = 0;
            }
        }
    }

    #endregion

    #region Push & Pull

    public void startPush(int direction, float aForce)
    {
        _isPushed = true;
        _pushDirection = direction;
        _pushForce = aForce;
    }

    void push()
    {
        if (_isPushed)
        {
            _velocity.x = _pushDirection * _pushForce;
            _isPushed = false;
        }
    }

    private void pushControl()
    {
        _pushControl.pushBehavior();
    }

    public void startPull(Vector2 direction, float aForce)
    {
        _isPulled = true;
        _pullDirection = direction;
        _pullForce = aForce;
    }

    public void stopPull()
    {
        _isPulled = false;
    }

    void pull()
    {
        if (_isPulled)
        {
            
            _velocity = _pullDirection * _pullForce;
            /*
            if (_pullDirection.x != 0)
            {
                if (_controller._collisionInfo.left || _controller._collisionInfo.right)
                {
                    _isPulled = false;
                }
            }
            if (Mathf.Abs(_pullDirection.y) == 1)
            {
                if (_controller._collisionInfo.below || _controller._collisionInfo.above)
                {
                    _isPulled = false;
                }
            }
            */
        }
    }

    #endregion

    #region Misc

    private void catchInput()
    {
        _input = _myVirtualJoystick.GetLeftStickClamped();
    }

    public int getFacingDirection()
    {
        return _controller._collisionInfo.faceDir;
    }

    public CVirtualJoystick getVirtualJoystick()
    {
        return _myVirtualJoystick;
    }

    public Vector2 getVelocityVector()
    {
        return _velocity;
    }
    
    public CharacterControler2D getCharacterControler()
    {
        return _controller;
    }

    public bool isWallSlideing()
    {
        return _wallSliding;
    }

    #endregion



    #region ColissionKiller
    void OnTriggerEnter2D(Collider2D other)
    {
        setState(State_Dead);
    }
    #endregion
}
