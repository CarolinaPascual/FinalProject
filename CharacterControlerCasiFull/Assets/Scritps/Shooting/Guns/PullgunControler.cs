using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullgunControler : GenericWeapon {

    public float _maxLenght;
    public float _pullForce;
    public GameObject _hookSpawnPoint;
  
    private bool _fired;
    private GameObject _lastHook;
    private BoxCollider2D _collider;
    private PlayerControler _hitTarget;
    private Vector2 _nextDiretion;
    private Vector2 _currentLenght;

    void Start ()
    {
        timeSinceLastShoot = shootCD;
    }

	void Update ()
    {
        hookControl();
        timeSinceLastShoot += Time.deltaTime;
    }

    public void hookControl()
    {
        if (_fired)
        {
            _currentLenght = transform.position - _lastHook.transform.position;
            if (_currentLenght.magnitude >= _maxLenght)
            {
				_lastHook.GetComponent<HookControler>().destoyHook();
                _fired = false;
            }
            if (_hitTarget != null)
            {
                Debug.Log("KJSADFAE");
                Vector2 v = transform.position - _hitTarget.transform.position;
                if (v.magnitude <= .5f)
                {
                   
                    _hitTarget = null;
                }
            }
        }
    }

    public void hit(PlayerControler p)
    {
        Debug.Log(p.transform.position);
        _hitTarget = p;
        Vector2 v = transform.position - p.transform.position;
        v.Normalize();
        p.startPull(v, _pullForce);
    }

    public override void fire(Vector2 direction)
    {
        if (timeSinceLastShoot >= shootCD)
        {
            if (!_fired)
            {
                _fired = true;
                GameObject auxBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                auxBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
                _lastHook = auxBullet;
				_lastHook.GetComponent<HookControler>()._owner = _owner;
				_lastHook.GetComponent<HookControler>().setRot(direction);
                timeSinceLastShoot = 0;
            }
        }
    }
}
