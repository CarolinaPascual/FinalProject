using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastControler : MonoBehaviour
{
    public LayerMask _collisionLayerMask;
    public LayerMask _collisionOnewayLayerMask;
    public float _skinWidth = .015f;

    [HideInInspector]
    public int _horizontalRayCount = 4;
    [HideInInspector]
    public int _verticalRayCount = 4;
    [HideInInspector]
    public float _horizontalRaySpacing;
    [HideInInspector]
    public float _verticalRaySpacing;
    [HideInInspector]
    public BoxCollider2D _collider;
    public RaycastOrigins _raycastOrigins;

    private const float dstBetweenRays = .25f;

    public virtual void Start()
    {
        _collisionLayerMask |= _collisionOnewayLayerMask;
        _collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public void CalculateRaySpacing()
    {
        Bounds bounds = _collider.bounds;
        bounds.Expand(_skinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        _horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
        _verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);

        _horizontalRaySpacing = bounds.size.y / (_horizontalRayCount - 1);
        _verticalRaySpacing = bounds.size.x / (_verticalRayCount - 1);
    }

    public void UpdateRaycastOrigins()
    {
        Bounds bounds = _collider.bounds;
        bounds.Expand(_skinWidth * -2);

        _raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        _raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        _raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        _raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

}
