using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterSelector : MonoBehaviour {

    private CVirtualJoystick _myVirtualJoystick;
    private bool _switching;
    private bool _isFastSwitching;
    private float _switchFastTimeCount;
    private float _nextFastSwitchTimeCount;
    private float _switchFastTime = 30;
    [HideInInspector]
    public int _currentSelection;

    private void Awake()
    {
        _myVirtualJoystick = new CVirtualJoystick();
        _myVirtualJoystick.init();
    }

    void Start ()
    {
        
	}
	
	void Update ()
    {
        scrollThroughSelections();
        startMatch();
    }

    private void startMatch()
    {
        if (_myVirtualJoystick._joystickNumber == 1)
        {
            if (_myVirtualJoystick.GetAction1Down())
            {
                SceneChanger.inst.goToPlayScene();
            }
        }
    }

    private void scrollThroughSelections()
    {
        if (_myVirtualJoystick.GetLeftStickClamped().x == 1)
        {
            if (!_switching)
            {
                _switching = true;
                CharacterSelector.inst.switchPlayerSelector(this, 1);
            }
            else
            {
                _switchFastTimeCount++;
                if (_switchFastTimeCount >= _switchFastTime)
                {
                    _nextFastSwitchTimeCount++;
                    if (_nextFastSwitchTimeCount >= 5)
                    {
                        CharacterSelector.inst.switchPlayerSelector(this, 1);
                        _nextFastSwitchTimeCount = 0;
                    }
                }
            }
        }
        else if (_myVirtualJoystick.GetLeftStickClamped().x == -1)
        {
            if (!_switching)
            {
                _switching = true;
                CharacterSelector.inst.switchPlayerSelector(this, -1);
            }
            else
            {
                _switchFastTimeCount++;
                if (_switchFastTimeCount >= _switchFastTime)
                {
                    _nextFastSwitchTimeCount++;
                    if (_nextFastSwitchTimeCount >= 5)
                    {
                        CharacterSelector.inst.switchPlayerSelector(this, -1);
                        _nextFastSwitchTimeCount = 0;
                    }
                }
            }
        }
        else if (Mathf.Abs(_myVirtualJoystick.GetLeftStickClamped().x) < 1)
        {
            if (_switching)
            {
                _switching = false;
                _switchFastTimeCount = 0;
                _nextFastSwitchTimeCount = 0;
            }
        }
    }

    public CVirtualJoystick getVirtualJoystick()
    {
        return _myVirtualJoystick;
    }

}
