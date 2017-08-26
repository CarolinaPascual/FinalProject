using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using System.Linq;

public class CInputManager : MonoBehaviour {

    #region Singleton stuff
    private static CInputManager _inst;
    public static CInputManager Inst
    {
        get
        {
            if (_inst == null)
                return new GameObject("Input Manager").AddComponent<CInputManager>();
            return _inst;
        }
    }
    #endregion

    #region Privates
    private List<InputDevice> _activeDevices;
    private KeyboardDevice _keyboardDevice;
    private bool _joyStickOneUsed;
    private bool _joyStickTwoUsed;
    private bool _joyStickThreeUsed;
    private bool _joyStickFourUsed;
    private bool _keyboardUsed;
    #endregion

    private void Awake()
    {
        init();
        
    }

    void Start ()
    {
        
	}
	
	
	void Update ()
    {
        updateActiveDevices();
	}

    private void updateActiveDevices()
    {
        _activeDevices = InputManager.Devices.ToList();
    }

    public InputDevice getFreeActiveDevice()
    {
        if (!_joyStickOneUsed)
        {
            _joyStickOneUsed = true;
            return _activeDevices[0];
        }
        if (!_joyStickTwoUsed)
        {
            _joyStickTwoUsed = true;
            return _activeDevices[1];
        }
        if (!_joyStickThreeUsed)
        {
            _joyStickThreeUsed = true;
            return _activeDevices[2];
        }
        if (!_joyStickFourUsed)
        {
            _joyStickFourUsed = true;
            return _activeDevices[3];
        }
        if (!_keyboardUsed)
        {
            _keyboardUsed = true;
            return _keyboardDevice;
        }
        return null;
    }

    private InputDevice GetKeyboardDevice()
    {
        for (int i = 0; i < _activeDevices.Count; i++)
        {
            if (_activeDevices[i].Name == "Keyboard device")
            {
                return _activeDevices[i];
            }
        }
        return null;
    }

    private InputDevice GetJoystickDevice()
    {
        for (int i = 0; i < _activeDevices.Count; i++)
        {
            if (_activeDevices[i].Name != "Keyboard device")
            {
                return _activeDevices[i];
            }
        }
        return null;
    }

    private void init()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _inst = this;
        _keyboardDevice = new KeyboardDevice();
        InputManager.AttachDevice(_keyboardDevice);
        _activeDevices = InputManager.Devices.ToList();
    }

}
