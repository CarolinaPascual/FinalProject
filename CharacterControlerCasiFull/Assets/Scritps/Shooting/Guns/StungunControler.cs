using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StungunControler : GenericWeapon {

    public float _maxRaylength;
    public int _durationInFrames;
    public GameObject _colliderObject;
    public GameObject _model;
    private BoxCollider2D _collider;
    private bool _fired;
    private int _fireCount;

	void Start ()
    {

        timeSinceLastShoot = shootCD;
        _collider = _colliderObject.GetComponent<BoxCollider2D>();
        _collider.enabled = false;
        _collider.size = new Vector3(_maxRaylength, .2f, 0);
        _collider.offset = new Vector3(_maxRaylength / 2, 0, 0);
    }

	void Update ()
    {
        resetColider();
        timeSinceLastShoot += Time.deltaTime;
    }

    public void resetColider()
    {
        if (_fired)
        {
            _fireCount++;
            if (_fireCount >= _durationInFrames)
            {
                _collider.enabled = false;
                _fired = false;
                _fireCount = 0;
            }
        }
    }

    public override void fire(Vector2 direction)
    {
        if (timeSinceLastShoot >= shootCD)
        {
            _fired = true;
            _collider.enabled = true;
            _colliderObject.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg);
            timeSinceLastShoot = 0;
        }
    }

}
