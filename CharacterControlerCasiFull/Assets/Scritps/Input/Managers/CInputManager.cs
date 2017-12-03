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
	private List<bool> _usedDevices;
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
		for (int i = 0; i < _activeDevices.Count; i++) 
		{
			if (_activeDevices [i].Name != _keyboardDevice.Name) 
			{
				if (_usedDevices[i] == false)
				{
					_usedDevices [i] = true;
					return _activeDevices [i];
				}
			}	
			else
			{
				if (_keyboardUsed == false)
				{
					_keyboardUsed = true;
					return _keyboardDevice;
				}
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
        DontDestroyOnLoad(gameObject);
        _keyboardDevice = new KeyboardDevice();
        InputManager.AttachDevice(_keyboardDevice);
        _activeDevices = InputManager.Devices.ToList();
		_usedDevices = new List<bool> (4);
		bools ();
    }

	private void bools()
	{
		bool a = false;
		bool b = false;
		bool c = false;
		bool d = false;
		_usedDevices.Add(a);
		_usedDevices.Add(b);
		_usedDevices.Add(c);
		_usedDevices.Add(d);
	}

    public void resetBools()
    {
        _usedDevices[0] = false;
        _usedDevices[1] = false;
        _usedDevices[2] = false;
        _usedDevices[3] = false;
        _keyboardUsed = false;
    }

}
