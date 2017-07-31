using UnityEngine;

public class TPCamera : MonoBehaviour
{
    public Transform Target;
    public LayerMask OccludersMask;

    public float LowAngleTargetDistance = .5f;
    public float ZoomOutDampening = 10;

    [Header("Constraints")]
    public float MaxDistance = 10;
    [Range(0, -90)]
    public float MinPitch = -70;
    [Range(0, 90)]
    public float MaxPitch = 70;

    [Header("Quality")]
    [Range(1, 30)]
    public int MaxOcclusionChecks = 25;

    private float yaw = 0;
    private float pitch = 0;
    private float distance = 0;
    private float goalDistance = 0;

    public Camera Camera { get; private set; }

    private void Awake()
    {
        this.Camera = this.GetComponent<Camera>();
 
        this.yaw = this.transform.localEulerAngles.y;
        this.pitch = this.transform.localEulerAngles.x;
        this.distance = this.MaxDistance;
    }

    private void LateUpdate()
    {
        // Update angles.
        this.transform.localEulerAngles = new Vector3(pitch, yaw, 0);

        // Update goal distance.
        this.goalDistance = this.TestForOcclusion();

        // Update distance.
        if (this.distance > this.goalDistance)
            this.distance = this.goalDistance;
        else
            this.distance = Mathf.Lerp(this.distance, this.goalDistance, Time.deltaTime * this.ZoomOutDampening);

        // Translate to the desired position.
        this.TranslateToLookAt(this.Target.position, this.distance);
    }

    private void TranslateToLookAt(Vector3 target, float distance)
    {
        this.transform.position = target;
        this.transform.Translate(Vector3.back * distance + Vector3.up * this.LowAngleTargetDistance * (this.pitch / -90));
    }

    private float TestForOcclusion()
    {
        if (!this.IsOccluded(this.MaxDistance))
            return this.MaxDistance;

        float low = 0;
        float high = this.MaxDistance;
        float best = 0;
        float prevMid = 0;
        for (int checks = 0; checks < this.MaxOcclusionChecks; checks++)
        {
            float mid = (low + high) / 2;

            if (Mathf.Approximately(mid, prevMid))
                break;

            prevMid = mid;

            if (this.IsOccluded(mid))
            {
                high = mid;
            }
            else
            {
                low = mid;
                best = mid;
            }
        }
        return best;
    }

    private bool IsOccluded(float distance)
    {
        // Set the distance from which to test.
        this.TranslateToLookAt(this.Target.position, distance);

        ClipPlane clipPlane = this.GetNearClipPlane(this.transform.position);
        RaycastHit hit;
        foreach (Vector3 planePoint in new Vector3[] {
            clipPlane.UpperLeft, clipPlane.LowerLeft, clipPlane.UpperRight, clipPlane.LowerRight,
            this.transform.position + this.transform.forward * -this.Camera.nearClipPlane
        })
        {
            if (Physics.Linecast(this.Target.position, planePoint, out hit, this.OccludersMask))
            {
                this.transform.position = this.Target.position;
                return true;
            }
        }

        this.transform.position = this.Target.position;
        return false;
    }

    private ClipPlane GetNearClipPlane(Vector3 position)
    {
        ClipPlane clipPlane = new ClipPlane();

        float distance = this.Camera.nearClipPlane;
        float height = distance * Mathf.Tan(this.Camera.fieldOfView / 2 * Mathf.Deg2Rad);
        float width = height * this.Camera.aspect;

        clipPlane.UpperLeft = position - this.transform.right * width + this.transform.up * height + this.transform.forward * distance;
        clipPlane.LowerLeft = position - this.transform.right * width - this.transform.up * height + this.transform.forward * distance;
        clipPlane.UpperRight = position + this.transform.right * width + this.transform.up * height + this.transform.forward * distance;
        clipPlane.LowerRight = position + this.transform.right * width - this.transform.up * height + this.transform.forward * distance;

        return clipPlane;
    }

    private struct ClipPlane
    {
        public Vector3 UpperLeft;
        public Vector3 LowerLeft;
        public Vector3 UpperRight;
        public Vector3 LowerRight;
    }
}