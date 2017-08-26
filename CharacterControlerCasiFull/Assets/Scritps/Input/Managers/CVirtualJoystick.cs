using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class CVirtualJoystick : MonoBehaviour {

    private InputDevice atachedDevice;
    private Vector2 _clampedVector;
    private bool _deviceConnected;
    public float _clampDeadZone = 0.5f;

    #region Sticks

    public Vector2 GetLeftStickVector()
    {
        return atachedDevice.LeftStick.Vector;
    }

    private float clampStickFloat(float aFloat)
    {
        if (Mathf.Abs(aFloat) < _clampDeadZone)
        {
            return 0;
        }
        float c = (aFloat > _clampDeadZone) ? 1 : -1;
        return c;
    }

    public Vector2 GetLeftStickClamped()
    {
        _clampedVector.x = clampStickFloat(atachedDevice.LeftStick.X);
        _clampedVector.y = clampStickFloat(atachedDevice.LeftStick.Y);
        return _clampedVector;
    }

    #endregion

    #region Actions

    #region FirstPressed

    public bool GetAction1Down()
    {
        return atachedDevice.Action1.WasPressed;
    }

    public bool GetAction2Down()
    {
        return atachedDevice.Action2.WasPressed;
    }

    public bool GetAction3Down()
    {
        return atachedDevice.Action3.WasPressed;
    }

    public bool GetAction4Down()
    {
        return atachedDevice.Action4.WasPressed;
    }

    #endregion

    #region Pressed

    public bool GetAction1Pressed()
    {
        return atachedDevice.Action1.IsPressed;
    }

    public bool GetAction2Pressed()
    {
        return atachedDevice.Action2.IsPressed;
    }

    public bool GetAction3Pressed()
    {
        return atachedDevice.Action3.IsPressed;
    }

    public bool GetAction4Pressed()
    {
        return atachedDevice.Action4.IsPressed;
    }

    #endregion

    #region Released

    public bool GetAction1Released()
    {
        return atachedDevice.Action1.WasReleased;
    }

    public bool GetAction2Released()
    {
        return atachedDevice.Action2.WasReleased;
    }

    public bool GetAction3Released()
    {
        return atachedDevice.Action3.WasReleased;
    }

    public bool GetAction4Released()
    {
        return atachedDevice.Action4.WasReleased;
    }

    #endregion

    #endregion

    #region Triggers & Bumpers

    #region FirstPressed

    public bool GetLeftTriggerDown()
    {
        return atachedDevice.LeftTrigger.WasPressed;
    }

    public bool GetRightTriggerDown()
    {
        return atachedDevice.RightTrigger.WasPressed;
    }

    public bool GetLeftBumperDown()
    {
        return atachedDevice.LeftBumper.WasPressed;
    }

    public bool GetRightBumperDown()
    {
        return atachedDevice.RightBumper.WasPressed;
    }

    #endregion

    #region Pressed

    public bool GetLeftTriggerPressed()
    {
        return atachedDevice.LeftTrigger.IsPressed;
    }

    public bool GetRightTriggerPressed()
    {
        return atachedDevice.RightTrigger.IsPressed;
    }

    public bool GetLeftBumperPressed()
    {
        return atachedDevice.LeftBumper.IsPressed;
    }

    public bool GetRightBumperPressed()
    {
        return atachedDevice.RightBumper.IsPressed;
    }

    #endregion

    #region Released

    public bool GetLeftTriggerReleased()
    {
        return atachedDevice.LeftTrigger.WasReleased;
    }

    public bool GetRightTriggerReleased()
    {
        return atachedDevice.RightTrigger.WasReleased;
    }

    public bool GetLeftBumperReleased()
    {
        return atachedDevice.LeftBumper.WasReleased;
    }

    public bool GetRightBumperReleased()
    {
        return atachedDevice.RightBumper.WasReleased;
    }

    #endregion

    #endregion

    public void init()
    {
        atachedDevice = CInputManager.Inst.getFreeActiveDevice();
    }

    #region TO DO
    private void updateDeviceStatus()
    {
        if (atachedDevice == null)
        {
            _deviceConnected = false;
        }
        else
        {
            _deviceConnected = true;
        }  
    }

    public void Update()
    {
        updateDeviceStatus();
    }

    public bool DeviceDetached()
    {
        return _deviceConnected;
    }
    #endregion
}
