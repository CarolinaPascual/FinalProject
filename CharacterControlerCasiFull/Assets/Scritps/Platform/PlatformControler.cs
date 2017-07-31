using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControler : RaycastControler {

    public LayerMask _passengerMask;
    [Header("Movement Variables")]
    public float _speed;
    public bool _cyclic;
    public float _waitTime;
    [Range (0,2)]
    public float _easeAmount;
    public Vector3[] _localWaypoints;
    
    private List<PassangerMovement> _passengerMovement;
    private Dictionary<Transform, CharacterControler2D> _passangerDictionary = new Dictionary<Transform, CharacterControler2D>();
    private Vector3[] _globalWaypoints;
    private int _fromWaypointIndex;
    private float _percentBetweenWaypoints;
    private float _nextMoveTime;

    public override void Start()
    {
        base.Start();
        _globalWaypoints = new Vector3[_localWaypoints.Length];
        for (int i = 0; i < _localWaypoints.Length; i++)
        {
            _globalWaypoints[i] = _localWaypoints[i] + transform.position;
        }
    }

    private void Update()
    {
        UpdateRaycastOrigins();
        
        Vector3 velocity = CalculatePlatformMovement();

        CalculatePassangerMovement(velocity);
        MovePassangers(true);
        transform.Translate(velocity);
        MovePassangers(false);
    }

    private Vector3 CalculatePlatformMovement()
    {
        if (Time.time < _nextMoveTime)
        {
            return Vector3.zero;
        }
        _fromWaypointIndex %= _globalWaypoints.Length;
        int toWaypointIndex = (_fromWaypointIndex + 1) % _globalWaypoints.Length;
        float distanceBetweenWaypoint = Vector3.Distance(_globalWaypoints[_fromWaypointIndex], _globalWaypoints[toWaypointIndex]);
        _percentBetweenWaypoints += Time.deltaTime * _speed/distanceBetweenWaypoint;
        _percentBetweenWaypoints = Mathf.Clamp01(_percentBetweenWaypoints);
        float easedPercentBetweenWaypoints = Ease(_percentBetweenWaypoints);

        Vector3 newPos = Vector3.Lerp(_globalWaypoints[_fromWaypointIndex], _globalWaypoints[toWaypointIndex], easedPercentBetweenWaypoints);

        if (_percentBetweenWaypoints >= 1)
        {
            _percentBetweenWaypoints = 0;
            _fromWaypointIndex++;
            if (!_cyclic)
            {
                if (_fromWaypointIndex >= _globalWaypoints.Length - 1)
                {
                    _fromWaypointIndex = 0;
                    System.Array.Reverse(_globalWaypoints);
                }
            }
            _nextMoveTime = Time.time + _waitTime;
        }

        return newPos - transform.position;
    }

    private float Ease(float x)
    {
        float a = _easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    private void MovePassangers(bool beforeMovePlatform)
    {
        foreach (PassangerMovement passenger in _passengerMovement)
        {
            if (!_passangerDictionary.ContainsKey(passenger.transform))
            {
                _passangerDictionary.Add(passenger.transform, passenger.transform.GetComponent<CharacterControler2D>());
            }
            if (passenger.moveBeforePlatform == beforeMovePlatform)
            {
                _passangerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
            }
        }
    }

    private void CalculatePassangerMovement(Vector3 velocity)
    {
        HashSet<Transform> movePassengers = new HashSet<Transform>();
        _passengerMovement = new List<PassangerMovement>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        //Verticaly moving platforms
        if (velocity.y != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + _skinWidth;

            for (int i = 0; i < _verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? _raycastOrigins.bottomLeft : _raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (_verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, _passengerMask);

                if (hit)
                {
                    if (!movePassengers.Contains(hit.transform))
                    {
                        movePassengers.Add(hit.transform);
                        float pushX = (directionY == 1) ? velocity.x : 0;
                        float pushY = velocity.y - (hit.distance - _skinWidth) * directionY;

                        _passengerMovement.Add(new PassangerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
                    }
                }
            }
        }

        //Horizontaly moving platforms
        if (velocity.x != 0)
        {
            float rayLength = Mathf.Abs(velocity.x) + _skinWidth;

            for (int i = 0; i < _horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? _raycastOrigins.bottomLeft : _raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (_horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, _passengerMask);

                if (hit)
                {
                    if (!movePassengers.Contains(hit.transform))
                    {
                        movePassengers.Add(hit.transform);
                        float pushX = velocity.x - (hit.distance - _skinWidth) * directionX;
                        float pushY = -_skinWidth;

                        _passengerMovement.Add(new PassangerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
                    }
                }

            }
        }

        //Passanger on top of a horizontaly or downward moving platform
        if (directionY == -1 || velocity.y == 0 && velocity.x != 0)
        {
            float rayLength = _skinWidth * 2;

            for (int i = 0; i < _verticalRayCount; i++)
            {
                Vector2 rayOrigin = _raycastOrigins.topLeft + Vector2.right * (_verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, _passengerMask);

                if (hit)
                {
                    if (!movePassengers.Contains(hit.transform))
                    {
                        movePassengers.Add(hit.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        _passengerMovement.Add(new PassangerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
                    }
                }
            }
        }
    }

    struct PassangerMovement
    {
        public Transform transform;
        public Vector3 velocity;
        public bool standingOnPlatform;
        public bool moveBeforePlatform;

        public PassangerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
        {
            transform = _transform;
            velocity = _velocity;
            standingOnPlatform = _standingOnPlatform;
            moveBeforePlatform = _moveBeforePlatform;
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
                Vector3 globalWaypointPos = (Application.isPlaying)?_globalWaypoints[i] : _localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        }
    }
}
