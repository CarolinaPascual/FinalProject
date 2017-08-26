using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterControler2D))]
public class PlayerControler : MonoBehaviour {

    [Header("Movement Variables")]
    public float _moveSpeed = 6;
    public float _jumpHeight = 4;
    public float _timeToJumpApex = .4f;
    public float _accelerationTimeAirborned = .2f;
    public float _accelerationTimeGrounded = .1f;
    [Header("Wallslide Variables")]
    public float _wallSlideSpeedMax = 3;
    public float _wallStickTime = .25f;

    [Header("Walljump Variables")]
    public Vector2 _wallJumpClimb;
    public Vector2 _wallJumpOff;
    public Vector2 _wallJumpLeap;

    private bool _wallSliding = false;
    private float _gravity;
    private float _jumpVelocity;
    private float _timeToWallUnstick;
    private float _velocityXDamper;
    private Vector3 _velocity;
    private Vector2 _input;
    private CharacterControler2D _controller;
    private bool isPushed = false;
    private int pushDirection;
    private float pushForce;
    private CVirtualJoystick _myVirtualJoystick;

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

    }



    void Update()
    {
        catchInput();
        smoothSpeed();
        wallSlide();

        if (_controller._collisionInfo.above || _controller._collisionInfo.below)
        {
            _velocity.y = 0;
        }

        jump();
        push();
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void catchInput()
    {
        _input = _myVirtualJoystick.GetLeftStickClamped();
    }

    private void smoothSpeed()
    {
        float targetVelocityX = _input.x * _moveSpeed;
        _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXDamper, (_controller._collisionInfo.below) ? _accelerationTimeGrounded : _accelerationTimeAirborned);
    }

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

    public void startPush(int direction, float aForce)
    {
        isPushed = true;
        pushDirection = direction;
        pushForce = aForce;
    }

    void push()
    {
        if (isPushed)
        {
            _velocity.x = pushDirection * pushForce;
            isPushed = false;
        }


    }

    public int getFacingDirection()
    {
        return _controller._collisionInfo.faceDir;
    }

    public CVirtualJoystick getVirtualJoystick()
    {
        return _myVirtualJoystick;
    }
}
