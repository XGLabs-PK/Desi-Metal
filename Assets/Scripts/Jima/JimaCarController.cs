using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JimaCarController : MonoBehaviour
{
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
    [Space]
    public Vector3 cameraRelativeOffset = Vector3.zero;
    public float cameraLerpFactor = 8.0f;

    private float vertical = 0.0f;
    private float horizontal = 0.0f;
    private bool jump = false;

    private float velocity = 0.0f;
    private float turning = 0.0f;

    private Camera cam;

    void Start()
    {
        if(rb == null) rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
        jump = Input.GetButtonDown("Jump");

        if(vertical != 0.0f)
        {
            velocity += (vertical * (vertical > 0.0f ? acceleration : deceleration)) * Time.deltaTime;
        }
        else
        {
            velocity += (velocity > 0.0f ? -1.0f : 1.0f) * slowDownFactor * Time.deltaTime;
            if(Mathf.Abs(velocity) < 1.0f) velocity = 0.0f;
        }
        velocity = Mathf.Clamp(velocity, -negativeSpeed, speed);

        if(horizontal != 0.0f)
        {
            turning += horizontal * turningAcceleration * Time.deltaTime;
        }
        else
        {
            turning += (turning > 0.0f ? -1.0f : 1.0f) * turningSlowDownFactor * Time.deltaTime;
            if(Mathf.Abs(turning) < 1.0f) turning = 0.0f;
        }
        turning = Mathf.Clamp(turning, -negativeTurningSpeed, turningSpeed);

        transform.position = Vector3.Lerp(transform.position, rb.position, transformSyncLerpFactor * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, rb.rotation, transformSyncLerpFactor * Time.deltaTime);

        cam.transform.position = Vector3.Lerp(cam.transform.position, transform.position + (transform.right * cameraRelativeOffset.x) + (transform.up * cameraRelativeOffset.y) + (transform.forward * cameraRelativeOffset.z), cameraLerpFactor * Time.deltaTime);
        cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, transform.rotation, cameraLerpFactor * Time.deltaTime);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + (transform.forward * velocity * Time.fixedDeltaTime));

        Vector3 rotation = rb.rotation.eulerAngles;
        rotation.y += turning * Time.fixedDeltaTime;
        rb.MoveRotation(Quaternion.Euler(rotation));

        if(jump)
        {
            rb.AddForce((transform.up * 1000.0f) + (transform.right * Random.Range(-250.0f, 250.0f)), ForceMode.Impulse);
            jump = false;
        }
    }
}
