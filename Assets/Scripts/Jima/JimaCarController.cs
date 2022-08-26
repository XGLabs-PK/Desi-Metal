using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JimaCarController : MonoBehaviour
{
    public Camera cam;
    public Rigidbody rb;
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

    private float vertical = 0.0f;
    private float horizontal = 0.0f;
    private bool forceRotate = false;

    private float velocity = 0.0f;
    private float turning = 0.0f;

    private float dt;
    private float fdt;

    private bool onGround = false;

    void Start()
    {
        if(rb == null) rb = GetComponent<Rigidbody>();

        dt = Time.deltaTime;
        fdt = Time.fixedDeltaTime;
    }

    void LateUpdate()
    {
        onGround = IsGrounded();

        dt = Mathf.Lerp(dt, Time.deltaTime, 0.05f);

        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
        forceRotate = Input.GetButtonDown("Jump");

        if(vertical != 0.0f && onGround)
        {
            velocity += (vertical * (vertical > 0.0f ? acceleration : deceleration)) * dt;
        }
        else
        {
            velocity += (velocity > 0.0f ? -1.0f : 1.0f) * slowDownFactor * dt;
            if(Mathf.Abs(velocity) < 1.0f) velocity = 0.0f;
        }
        velocity = Mathf.Clamp(velocity, -negativeSpeed, speed);

        if(horizontal != 0.0f && onGround && Mathf.Abs(velocity) > minTurnVelocity)
        {
            turning += horizontal * turningAcceleration * dt;
        }
        else
        {
            turning += (turning > 0.0f ? -1.0f : 1.0f) * turningSlowDownFactor * dt;
            if(Mathf.Abs(turning) < 1.0f) turning = 0.0f;
        }
        turning = Mathf.Clamp(turning, -negativeTurningSpeed, turningSpeed);

        transform.position = Vector3.Lerp(transform.position, rb.position, transformSyncLerpFactor * dt);
        transform.rotation = Quaternion.Slerp(transform.rotation, rb.rotation, transformSyncLerpFactor * dt);

        cam.transform.position = Vector3.Lerp(cam.transform.position, transform.position + (transform.right * cameraRelativeOffset.x) + (transform.up * cameraRelativeOffset.y) + (transform.forward * cameraRelativeOffset.z), cameraLerpFactor * dt);
        cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, transform.rotation, cameraLerpFactor * dt);
    }

    void FixedUpdate()
    {
        fdt = Mathf.Lerp(fdt, Time.fixedDeltaTime, 0.05f);

        if(!onGround && Mathf.Abs(rb.velocity.y) < 50.0f) rb.velocity += Vector3.down * gravity * fdt;
        
        rb.MovePosition(rb.position + (transform.forward * velocity * fdt));

        Vector3 rotation = rb.rotation.eulerAngles;
        rotation.y += turning * fdt;
        rb.MoveRotation(Quaternion.Euler(rotation));

        if(forceRotate)
        {
            rb.AddTorque((transform.right * -6000.0f), ForceMode.Impulse);
            forceRotate = false;
        }
    }

    private bool IsGrounded()
    {
        foreach(var t in groundCheckTransforms)
            if(Physics.CheckSphere(t.position + (Vector3.down * groundCheckDistance), groundCheckRadius, groundCheckLayer))
                return true;
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach(var t in groundCheckTransforms)
            Gizmos.DrawWireSphere(t.position + (Vector3.down * groundCheckDistance), groundCheckRadius);
    }
}
