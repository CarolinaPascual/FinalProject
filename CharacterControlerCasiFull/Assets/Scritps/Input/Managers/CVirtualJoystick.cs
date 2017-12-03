using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class CVirtualJoystick : MonoBehaviour {

	private InputDevice _atachedDevice;
    private Vector2 _clampedVector;
    private bool _deviceConnected = true;
    public float _clampDeadZone = 0.5f;
    public int _joystickNumber;
    private string _lastDeviceName;

    #region Sticks

    public Vector2 GetLeftStickVector()
    {
        return _atachedDevice.LeftStick.Vector;
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
        _clampedVector.x = clampStickFloat(_atachedDevice.LeftStick.X);
        _clampedVector.y = clampStickFloat(_atachedDevice.LeftStick.Y);
        return _clampedVector;
    }

    #endregion

    #region Actions

    #region FirstPressed

    public bool GetAction1Down()
    {
        return _atachedDevice.Action1.WasPressed;
    }

    public bool GetAction2Down()
    {
        return _atachedDevice.Action2.WasPressed;
    }

    public bool GetAction3Down()
    {
        return _atachedDevice.Action3.WasPressed;
    }

    public bool GetAction4Down()
    {
        return _atachedDevice.Action4.WasPressed;
    }

    #endregion

    #region Pressed

    public bool GetAction1Pressed()
    {
        return _atachedDevice.Action1.IsPressed;
    }

    public bool GetAction2Pressed()
    {
        return _atachedDevice.Action2.IsPressed;
    }

    public bool GetAction3Pressed()
    {
        return _atachedDevice.Action3.IsPressed;
    }

    public bool GetAction4Pressed()
    {
        return _atachedDevice.Action4.IsPressed;
    }

    #endregion

    #region Released

    public bool GetAction1Released()
    {
        return _atachedDevice.Action1.WasReleased;
    }

    public bool GetAction2Released()
    {
        return _atachedDevice.Action2.WasReleased;
    }

    public bool GetAction3Released()
    {
        return _atachedDevice.Action3.WasReleased;
    }

    public bool GetAction4Released()
    {
        return _atachedDevice.Action4.WasReleased;
    }

    #endregion

    #endregion

    #region Triggers & Bumpers

    #region FirstPressed

    public bool GetLeftTriggerDown()
    {
        return _atachedDevice.LeftTrigger.WasPressed;
    }

    public bool GetRightTriggerDown()
    {
        return _atachedDevice.RightTrigger.WasPressed;
    }

    public bool GetLeftBumperDown()
    {
        return _atachedDevice.LeftBumper.WasPressed;
    }

    public bool GetRightBumperDown()
    {
        return _atachedDevice.RightBumper.WasPressed;
    }

    #endregion

    #region Pressed

    public bool GetLeftTriggerPressed()
    {
        return _atachedDevice.LeftTrigger.IsPressed;
    }

    public bool GetRightTriggerPressed()
    {
        return _atachedDevice.RightTrigger.IsPressed;
    }

    public bool GetLeftBumperPressed()
    {
        return _atachedDevice.LeftBumper.IsPressed;
    }

    public bool GetRightBumperPressed()
    {
        return _atachedDevice.RightBumper.IsPressed;
    }

    #endregion

    #region Released

    public bool GetLeftTriggerReleased()
    {
        return _atachedDevice.LeftTrigger.WasReleased;
    }

    public bool GetRightTriggerReleased()
    {
        return _atachedDevice.RightTrigger.WasReleased;
    }

    public bool GetLeftBumperReleased()
    {
        return _atachedDevice.LeftBumper.WasReleased;
    }

    public bool GetRightBumperReleased()
    {
        return _atachedDevice.RightBumper.WasReleased;
    }

    #endregion

    #region Values

    public float GetLeftTrigerValue()
    {
        return _atachedDevice.LeftTrigger.Value;
    }

    public float GetRightTrigerValue()
    {
        return _atachedDevice.RightTrigger.Value;
    }

    #endregion

    #endregion

    public void init()
    {
        _atachedDevice = CInputManager.Inst.getFreeActiveDevice();
		if (_atachedDevice != null)
		{
			_lastDeviceName = _atachedDevice.Name;
            _joystickNumber = InputManager.Devices.IndexOf(_atachedDevice) + 1;
            InputManager.OnDeviceDetached += deviceDetached;
			InputManager.OnDeviceAttached += deviceAttached;
		}
    }

    #region Attach & Detach

    private void deviceDetached(InputDevice a)
    {
        if (a == _atachedDevice)
        {
            _deviceConnected = false;
        }
        else
        {
            _joystickNumber = InputManager.Devices.IndexOf(_atachedDevice) + 1;
        }
    }

    private void deviceAttached(InputDevice a)
    {
        if (!_deviceConnected && _lastDeviceName == a.Name)
        {
            _atachedDevice = a;
            _deviceConnected = true;
            _joystickNumber = InputManager.Devices.IndexOf(_atachedDevice) + 1;
        }
    }

	public InputDevice getAtachedDevice()
	{
		return _atachedDevice;
	}

    public void setAtachedDevice(InputDevice inputDevice)
    {
        _atachedDevice = inputDevice;
    }
    #endregion
}
