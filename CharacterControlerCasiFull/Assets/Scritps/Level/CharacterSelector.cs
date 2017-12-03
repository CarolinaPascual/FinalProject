using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class CharacterSelector : MonoBehaviour {

    public Vector3[] _localWaypoints;
    [HideInInspector]
    public static CharacterSelector inst;

    private int _devicesConected;
    private Vector3[] _globalWaypoints;

    private void Awake()
    {
        inst = this;
    }

    void Start ()
    {
        _globalWaypoints = new Vector3[_localWaypoints.Length];
        for (int i = 0; i < _localWaypoints.Length; i++)
        {
            _globalWaypoints[i] = _localWaypoints[i] + transform.position;
        }
        _devicesConected = InputManager.Devices.Count;
        turnSelectorsOn();
    }
	
	void Update ()
    {
		
	}

    private void turnSelectorsOn()
    {
        for (int i = 1; i < _devicesConected+1; i++)
        {
            GameObject child = transform.FindChild("P" + i + "Selector").gameObject;
            child.SetActive(true);
            child.transform.position = _globalWaypoints[i-1];
            child.GetComponent<PlayerCharacterSelector>()._currentSelection = i;
            PlayerPrefs.SetInt("PlayerModel" + i, i);
            PlayerPrefs.SetInt("PlayerAmount", i);
        }
    }

    public void switchPlayerSelector(PlayerCharacterSelector playerSelector, int direcction)
    {
        if (direcction == 1)
        {
            if (playerSelector._currentSelection == 4)
            {
                playerSelector._currentSelection = 1;
                playerSelector.gameObject.transform.position = _globalWaypoints[0];
                PlayerPrefs.SetInt("PlayerModel" + playerSelector.getVirtualJoystick()._joystickNumber, playerSelector._currentSelection);
            }
            else
            {
                playerSelector._currentSelection = playerSelector._currentSelection + 1;
                playerSelector.gameObject.transform.position = _globalWaypoints[playerSelector._currentSelection - 1];
                PlayerPrefs.SetInt("PlayerModel" + playerSelector.getVirtualJoystick()._joystickNumber, playerSelector._currentSelection);
            }
        }
        else if (direcction == -1)
        {
            if (playerSelector._currentSelection == 1)
            {
                playerSelector._currentSelection = 4;
                playerSelector.gameObject.transform.position = _globalWaypoints[3];
                PlayerPrefs.SetInt("PlayerModel" + playerSelector.getVirtualJoystick()._joystickNumber, playerSelector._currentSelection);
            }
            else
            {
                playerSelector._currentSelection = playerSelector._currentSelection - 1;
                playerSelector.gameObject.transform.position = _globalWaypoints[playerSelector._currentSelection - 1];
                PlayerPrefs.SetInt("PlayerModel" + playerSelector.getVirtualJoystick()._joystickNumber, playerSelector._currentSelection);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_localWaypoints != null)
        {
            Gizmos.color = Color.red;
            float size = .3f;
            for (int i = 0; i < _localWaypoints.Length; i++)
            {
                Vector3 globalWaypointPos = (Application.isPlaying) ? _globalWaypoints[i] : _localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        }
    }
}
