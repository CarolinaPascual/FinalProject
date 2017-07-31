using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterControler2D : RaycastControler {

    #region privates
    private float _maxClimbAngle = 80;
    private float _maxDescendAngle = 75;
    public CollisionInfo _collisionInfo;
    [HideInInspector]
    public bool _ignoreOneWayPlatformsThisFrame;
    #endregion

    public override void Start()
    {
        base.Start();
        _collisionInfo.faceDir = 1;
    }

    #region Public methods

    public void Move(Vector2 moveAmount, bool standingOnPlatform = false)
    {
        UpdateRaycastOrigins();
        _collisionInfo.Reset();
        _collisionInfo.moveAmountOld = moveAmount;

        if (moveAmount.x != 0)
        {
            _collisionInfo.faceDir = (int)Mathf.Sign(moveAmount.x);
        }

        if (moveAmount.y < 0)
        {
            DescendSlope(ref moveAmount);
        }

        HorizontalCollisions(ref moveAmount);

        if (moveAmount.y != 0)
        {
            VerticalCollisions(ref moveAmount);
        }

        transform.Translate(moveAmount);

        if (standingOnPlatform)
        {
            _collisionInfo.below = true;
        }
    }

    #endregion

    #region Collisions

    private void VerticalCollisions(ref Vector2 moveAmount)
    {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + _skinWidth;

        var mask = _collisionLayerMask;
        if (directionY == 1)
        {
            mask &= ~_collisionOnewayLayerMask;
            _ignoreOneWayPlatformsThisFrame = false;
        }

        for (int i = 0; i < _verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? _raycastOrigins.bottomLeft : _raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (_verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, mask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (hit)
            {
                if (checkForPlatformCollisionIgnoring(hit))
                {
                    return;
                }
                else
                {
                    moveAmount.y = (hit.distance - _skinWidth) * directionY;
                    rayLength = hit.distance;
                    if (_collisionInfo.climbingSlope)
                    {
                        moveAmount.x = moveAmount.y / Mathf.Tan(_collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
                    }

                    _collisionInfo.below = directionY == -1;
                    _collisionInfo.above = directionY == 1;
                    _collisionInfo.lastBelowPlatform = hit.transform.gameObject;
                    _ignoreOneWayPlatformsThisFrame = false;
                }
                
            }
        }

        if (_collisionInfo.climbingSlope)
        {
            float directionX = Mathf.Sign(moveAmount.x);
            rayLength = Mathf.Abs(moveAmount.x) + _skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? _raycastOrigins.bottomLeft : _raycastOrigins.bottomRight) + Vector2.up * moveAmount.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, mask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != _collisionInfo.slopeAngle)
                {
                    moveAmount.x = (hit.distance - _skinWidth) * directionX;
                    _collisionInfo.slopeAngle = slopeAngle;
                }
            }
        }
    }

    private void HorizontalCollisions(ref Vector2 moveAmount)
    {
        float directionX = _collisionInfo.faceDir;
        float rayLength = Mathf.Abs(moveAmount.x) + _skinWidth;

        if (Mathf.Abs(moveAmount.x) < _skinWidth)
        {
            rayLength = 2 * _skinWidth;
        }

        for (int i = 0; i < _horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? _raycastOrigins.bottomLeft : _raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (_horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, _collisionLayerMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit)
            {
                if (hit.distance == 0)
                {
                    continue;
                }

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= _maxClimbAngle)
                {
                    if (_collisionInfo.descendingSlope)
                    {
                        _collisionInfo.descendingSlope = false;
                        moveAmount = _collisionInfo.moveAmountOld;
                    }
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != _collisionInfo.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - _skinWidth;
                        moveAmount.x -= distanceToSlopeStart * directionX;
                    }
                    ClimeSlope(ref moveAmount, slopeAngle);
                    moveAmount.x += distanceToSlopeStart * directionX;
                }

                if (!_collisionInfo.climbingSlope || slopeAngle > _maxClimbAngle)
                {
                    moveAmount.x = (hit.distance - _skinWidth) * directionX;
                    rayLength = hit.distance;

                    if (_collisionInfo.climbingSlope)
                    {
                        moveAmount.y = Mathf.Tan(_collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                    }

                    _collisionInfo.left = directionX == -1;
                    _collisionInfo.right = directionX == 1;
                }
            }
        }
    }

    private bool checkForPlatformCollisionIgnoring(RaycastHit2D aHit)
    {
        if ((aHit.transform.gameObject == _collisionInfo.lastBelowPlatform) && _ignoreOneWayPlatformsThisFrame &&
            _collisionOnewayLayerMask == (_collisionOnewayLayerMask | (1 << aHit.transform.gameObject.layer)))
            //&& LayerMask.GetMask(LayerMask.LayerToName(aHit.transform.gameObject.layer)) == _collisionOnewayLayerMask.value)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region Slopes

    private void ClimeSlope(ref Vector2 moveAmount, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(moveAmount.x);
        float climbmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (moveAmount.y <= climbmoveAmountY)
        {
            moveAmount.y = climbmoveAmountY;
            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
            _collisionInfo.below = true;
            _collisionInfo.climbingSlope = true;
            _collisionInfo.slopeAngle = slopeAngle;
        }

    }

    private void DescendSlope(ref Vector2 moveAmount)
    {
        float directionX = Mathf.Sign(moveAmount.x);
        Vector2 rayOrigin = (directionX == -1) ? _raycastOrigins.bottomRight : _raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, _collisionLayerMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= _maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - _skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x))
                    {
                        float moveDistance = Mathf.Abs(moveAmount.x);
                        float descendmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                        moveAmount.y -= descendmoveAmountY;

                        _collisionInfo.slopeAngle = slopeAngle;
                        _collisionInfo.descendingSlope = true;
                        _collisionInfo.below = true;
                    }
                }
            }
        }
    }

    #endregion

    #region Structs
    
    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public int faceDir;
        public Vector2 moveAmountOld;

        public GameObject lastBelowPlatform;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }

    #endregion
}
