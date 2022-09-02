using UnityEngine;

public class JimaCarController : MonoBehaviour
{
    public Camera cam;
    public Rigidbody rb;
    public Transform flatTransform;
    [Space]
    public float transformSyncLerpFactor = 12.0f;
    [Space]
    public float speed = 10.0f;
    public float negativeSpeed = 10.0f;
    public float acceleration = 0.1f;
    public float deceleration = 0.1f;
    public float slowDownFactor = 1.0f;
    [Space]
    public float turningSpeed = 1.0f;
    public float negativeTurningSpeed = 1.0f;
    public float turningAcceleration = 0.1f;
    public float turningSlowDownFactor = 1.0f;
    public float minTurnVelocity = 1.0f;
    [Space]
    public float gravity = 10.0f;
    [Space]
    public float groundCheckDistance = 0.5f;
    public Transform[] groundCheckTransforms;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundCheckLayer;
    [Space]
    public Vector3 cameraRelativeOffset = Vector3.zero;
    public float cameraLerpFactor = 8.0f;

    float dt;
    float fdt;
    float horizontal;

    bool onGround;
    bool respawn;
    float turning;

    float velocity;

    float vertical;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        dt = Time.deltaTime;
        fdt = Time.fixedDeltaTime;
    }

    void Update()
    {
        onGround = IsGrounded();

        dt = Mathf.Lerp(dt, Time.deltaTime, 0.05f);

        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
        respawn = Input.GetButtonDown("Jump");

        if (vertical != 0.0f && onGround)
            velocity += vertical * (vertical > 0.0f ? acceleration : deceleration) * dt;
        else
        {
            velocity += (velocity > 0.0f ? -1.0f : 1.0f) * slowDownFactor * dt;
            if (Mathf.Abs(velocity) < 1.0f) velocity = 0.0f;
        }

        velocity = Mathf.Clamp(velocity, -negativeSpeed, speed);

        if (horizontal != 0.0f && Mathf.Abs(velocity) > minTurnVelocity && onGround)
            turning += horizontal * turningAcceleration * dt;
        else
        {
            turning += (turning > 0.0f ? -1.0f : 1.0f) * turningSlowDownFactor * dt;
            if (Mathf.Abs(turning) < 1.0f) turning = 0.0f;
        }

        turning = Mathf.Clamp(turning, -negativeTurningSpeed, turningSpeed);

        transform.position = Vector3.Lerp(transform.position, rb.position, transformSyncLerpFactor * dt);
        transform.rotation = Quaternion.Slerp(transform.rotation, rb.rotation, transformSyncLerpFactor * dt);

        flatTransform.position = transform.position;
        Vector3 flatRotation = transform.rotation.eulerAngles;
        flatRotation.x = flatRotation.z = 0.0f;
        flatTransform.rotation = Quaternion.Euler(flatRotation);
    }

    void FixedUpdate()
    {
        fdt = Mathf.Lerp(fdt, Time.fixedDeltaTime, 0.05f);

        if (!onGround && Mathf.Abs(rb.velocity.y) < 50.0f) rb.velocity += Vector3.down * gravity * fdt;

        rb.MovePosition(rb.position + (onGround ? flatTransform.forward : rb.transform.forward) * velocity * fdt);

        rb.drag = Mathf.Lerp(rb.drag, onGround ? 0.5f : 0.0f, 4.0f * fdt);
        rb.angularDrag = Mathf.Lerp(rb.angularDrag, onGround ? 1.0f : 0.05f, 4.0f * fdt);

        Vector3 rotation = rb.rotation.eulerAngles;
        rotation.y += turning * fdt;
        rb.MoveRotation(Quaternion.Euler(rotation));

        if (respawn)
        {
            rb.position += Vector3.up * 4.0f;
            rb.rotation = Quaternion.identity;

            respawn = false;
        }
    }

    void LateUpdate()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position,
            transform.position + transform.right * cameraRelativeOffset.x + transform.up * cameraRelativeOffset.y +
            transform.forward * cameraRelativeOffset.z, cameraLerpFactor * dt);

        cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, transform.rotation, cameraLerpFactor * dt);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        foreach (Transform t in groundCheckTransforms)
            Gizmos.DrawWireSphere(t.position + Vector3.down * groundCheckDistance, groundCheckRadius);
    }

    bool IsGrounded()
    {
        foreach (Transform t in groundCheckTransforms)
            if (Physics.CheckSphere(t.position + Vector3.down * groundCheckDistance, groundCheckRadius,
                    groundCheckLayer))
                return true;

        return false;
    }
}
