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

    public bool GetAction1Down()
    {
        return atachedDevice.Action1.WasPressed;
    }

    public bool GetAction2Down()
    {
        return atachedDevice.Action1.WasPressed;
    }

    #endregion
    
    #region Triggers & Bumpers
    public bool GetLeftTriggerDown()
    {
        return atachedDevice.LeftTrigger.WasPressed;
    }
    public bool GetRightTriggerDown()
    {
        return atachedDevice.RightTrigger.WasPressed;
    }
    #endregion

    public void init()
    {
        atachedDevice = CInputManager.Inst.getFreeActiveDevice();
    }

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
        GetLeftStickClamped();
    }

    public bool DeviceDetached()
    {
        return _deviceConnected;
    }
}
