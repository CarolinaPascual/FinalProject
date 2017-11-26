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
	private float _durationCount;
	public LineRenderer _lineRenderer;
	public int _lineLifeInFrames;
	private int _lineLifeCount;
	private bool _lineEnabled;

	void Start ()
    {

        timeSinceLastShoot = shootCD;
        _collider = _colliderObject.GetComponent<BoxCollider2D>();
        _collider.enabled = false;
        _collider.size = new Vector3(_maxRaylength, .2f, 0);
        _collider.offset = new Vector3(_maxRaylength / 2, 0, 0);
		_durationCount = duration;
		_controler = _owner.GetComponent<ShootingController>();
    }

	void Update ()
    {
		_durationCount -= Time.deltaTime;
		if (_durationCount <= 0)
		{
			_controler.clearWeapoon();
		}

		if (_lineEnabled) 
		{
			_lineLifeCount++;
			if (_lineLifeCount > _lineLifeInFrames) 
			{
				_lineRenderer.enabled = false;
				_lineEnabled = false;
				_lineLifeCount = 0;
			}
		}

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
			_collider.gameObject.GetComponent<StunControler> ()._owner = _owner;
			renderLine (direction);
        }
    }

	private void renderLine(Vector3 direction)
	{
		_lineRenderer.enabled = true;
		_lineRenderer.SetPosition (0, _collider.gameObject.transform.position);
		_lineRenderer.SetPosition (1, direction * _maxRaylength);
		_lineEnabled = true;
	}

}
