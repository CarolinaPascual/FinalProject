using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour {

	public GameObject _weaponToSpawn;
    public GameObject _crateModel;
	public float _minTimeToOpen;
	public float _maxTimeToOpen;

	public float _moveSpeed;
	public float _sineSize;
	public float _rotateSpeed;

	private float _timeToOpen;
	private bool _isSpawned;
    private bool _isOpened;
	private GameObject _weaponModel;
    private Animator _anim;
	private GameObject _lastWeaponModel;
	private Vector3 _pos;

	void Start () 
	{
		_timeToOpen = Random.Range (_minTimeToOpen, _maxTimeToOpen);
		_weaponModel = _weaponToSpawn.transform.Find("Model").gameObject;
        _anim = _crateModel.GetComponent<Animator>();

    }

	void Update () 
	{
		checkOpenCrate ();
		moveModel ();
        spawnWeaponCheck();
	}

    private void spawnWeaponCheck()
    {
        if (_isOpened && !_isSpawned)
        {
            if(_anim.GetCurrentAnimatorStateInfo(0).IsName("OpenCrate"))
            {
                if (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    spawnWeapon();
                }
            }
        }
    }

	private void checkOpenCrate()
	{
		_timeToOpen = _timeToOpen - Time.deltaTime;
		if (_timeToOpen <= 0) 
		{
			if (!_isOpened) 
			{
                _anim.Play("OpenCrate");
                _isOpened = true;
			}
		}
	}

	void OnTriggerStay2D(Collider2D collision)
	{
		PlayerControler a = collision.GetComponent<PlayerControler>();
		if (a != null) 
		{
			if (a.getVirtualJoystick ().GetAction4Down ()) 
			{
				a.GetComponent<ShootingController> ().equipWeapon (_weaponToSpawn);
				Destroy (_lastWeaponModel);
				Destroy (gameObject);
			}
		}
	}

	private void spawnWeapon()
	{
		_lastWeaponModel = Instantiate (_weaponModel, transform.position, Quaternion.identity);
		_isSpawned = true;
		_pos = _lastWeaponModel.transform.position;
        _crateModel.SetActive(false);
	}

    private void moveModel()
    {
        if (_isSpawned)
        {
            _lastWeaponModel.transform.position = _pos + transform.up * Mathf.Sin(Time.time * _moveSpeed) * _sineSize;
            _lastWeaponModel.transform.Rotate(new Vector3(0, _rotateSpeed, 0));
        }
    }
}
