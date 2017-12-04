using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatScript : MonoBehaviour {

	[Header("States")]
	public bool _normalStatePlayer;
	public bool _stunPlayer;
	public bool _slowPlayer;
	public bool _inputCursePlayer;
	public bool _jumpCursePlayer;
	[Header("Guns")]
	public bool _clearGun;
	public bool _giveStungun;
	public bool _giveSlowgun;
	public bool _givePullgun;
	public GameObject _stunGun;
	public GameObject _slowGun;
	public GameObject _pullGun;

	private PlayerControler _controler;
	private ShootingController _gunsControler;

	void Start () 
	{
		_controler = GetComponent<PlayerControler> ();
		_gunsControler = GetComponent<ShootingController>();
	}

	void Update () 
	{
        if (!LevelManager.Inst.isGamePaused)
        {
            if (_stunPlayer)
            {
                _controler.setState(_controler.State_Stuned, 60);
                _stunPlayer = false;
            }
            if (_slowPlayer)
            {
                _controler.setState(_controler.State_Slowed, 60);
                _slowPlayer = false;
            }
            if (_inputCursePlayer)
            {
                _controler.setState(_controler.State_InputCursed, 60);
                _inputCursePlayer = false;
            }
            if (_jumpCursePlayer)
            {
                _controler.setState(_controler.State_JumpCursed, 60);
                _jumpCursePlayer = false;
            }
            if (_normalStatePlayer)
            {
                _controler.setState(_controler.State_Normal);
                _normalStatePlayer = false;
            }
            if (_clearGun)
            {
                _gunsControler.clearWeapoon();
                _clearGun = false;
            }
            if (_giveStungun)
            {
                _gunsControler.equipWeapon(_stunGun);
                _giveStungun = false;
            }
            if (_giveSlowgun)
            {
                _gunsControler.equipWeapon(_slowGun);
                _giveSlowgun = false;
            }
            if (_givePullgun)
            {
                _gunsControler.equipWeapon(_pullGun);
                _givePullgun = false;
            }
        }
    }
}
