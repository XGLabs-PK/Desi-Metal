using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JimaSphereCarController : MonoBehaviour
{
    public Camera cam;
    public Rigidbody rb;
    [Space]
    public float transformSyncLerpFactor = 12.0f;
    [Space]
    public float speed = 100.0f;
    public float turningSpeed = 2.0f;
    public float gravity = 10.0f;
    [Space]
    public Vector3 cameraRelativeOffset = Vector3.zero;
    public float cameraLerpFactor = 8.0f;
    [Space]
    public float groundCheckDistance = 0.5f;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundCheckLayer;

    private float vertical = 0.0f;
    private float horizontal = 0.0f;

    private float dt;
    private float fdt;

    private bool onGround = false;

    private float currentSpeed = 0.0f;
    private float currentTurningSpeed = 0.0f;

    private Vector3 groundNormal = Vector3.up;
    private Vector3 prevPosition;

    void Start()
    {
        if(rb == null) rb = GetComponent<Rigidbody>();

        dt = Time.deltaTime;
        fdt = Time.fixedDeltaTime;

        prevPosition = transform.position;
    }

    void Update()
    {
        dt = Mathf.Lerp(dt, Time.deltaTime, 0.05f);

        onGround = IsGrounded(dt);

        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
        if(!onGround) vertical = horizontal = 0.0f;

        transform.position = Vector3.Lerp(transform.position, rb.position, transformSyncLerpFactor * dt);

        currentSpeed = Mathf.Lerp(currentSpeed, speed * vertical, 4.0f * dt);
        currentTurningSpeed = Mathf.Lerp(currentTurningSpeed, turningSpeed * horizontal, 4.0f * dt);

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y += currentTurningSpeed * dt;
        transform.rotation = Quaternion.Euler(rotation);
    }

    void FixedUpdate()
    {
        fdt = Mathf.Lerp(fdt, Time.fixedDeltaTime, 0.05f);

        //rb.velocity = Vector3.Lerp(rb.velocity, (transform.forward * currentSpeed) + (transform.right * currentTurningSpeed) + (Vector3.down * gravity * (onGround ? 1.0f : 0.0f)), 2.0f * fdt);
        rb.AddForce((transform.forward * currentSpeed) + (transform.right * currentTurningSpeed) + (Vector3.down * gravity * (onGround ? 1.0f : 0.0f)), ForceMode.Impulse);
    }

    void LateUpdate()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, transform.position + (transform.right * cameraRelativeOffset.x) + (transform.up * cameraRelativeOffset.y) + (transform.forward * cameraRelativeOffset.z), cameraLerpFactor * dt);
        cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, transform.rotation, cameraLerpFactor * dt);

        transform.rotation = Quaternion.LookRotation(transform.forward, groundNormal);

        prevPosition = transform.position;
    }

    private bool IsGrounded(float dt)
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position + (Vector3.down * groundCheckDistance), Vector3.down, out hit, groundCheckRadius, groundCheckLayer))
        {
            groundNormal = hit.normal;
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position + (Vector3.down * groundCheckDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(pos, pos + (Vector3.down * groundCheckRadius));
    }
}
