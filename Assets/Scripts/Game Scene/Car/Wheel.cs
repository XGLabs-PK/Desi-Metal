using System;
using PG_Physics.Wheel;
using UnityEngine;

/// <summary>
///     Wheel settings and update logic.
/// </summary>
[Serializable]
public struct Wheel
{
    const int SmoothValuesCount = 3;
    public WheelCollider WheelCollider;
    public Transform WheelView;
    public float SlipForGenerateParticle;
    public Vector3 TrailOffset;

    WheelHit Hit;

    //FXController FXController { get { return FXController.Instance; } }
    Vector3 HitPoint;

    PG_WheelCollider m_PGWC;
    TrailRenderer Trail;

    public float CurrentMaxSlip => Mathf.Max(CurrentForwardSleep, CurrentSidewaysSleep);
    public float CurrentForwardSleep { get; private set; }
    public float CurrentSidewaysSleep { get; private set; }
    public WheelHit GetHit => Hit;
    public PG_WheelCollider PG_WheelCollider
    {
        get
        {
            if (m_PGWC == null)
                m_PGWC = WheelCollider.GetComponent<PG_WheelCollider>();

            if (m_PGWC == null)
            {
                m_PGWC = WheelCollider.gameObject.AddComponent<PG_WheelCollider>();
                m_PGWC.CheckFirstEnable();
            }

            return m_PGWC;
        }
    }

    /// <summary>
    ///     Update gameplay logic.
    /// </summary>
    public void FixedUpdate()
    {

        if (WheelCollider.GetGroundHit(out Hit))
        {
            float prevForwar = CurrentForwardSleep;
            float prevSide = CurrentSidewaysSleep;

            CurrentForwardSleep = (prevForwar + Mathf.Abs(Hit.forwardSlip)) / 2;
            CurrentSidewaysSleep = (prevSide + Mathf.Abs(Hit.sidewaysSlip)) / 2;
        }
        else
        {
            CurrentForwardSleep = 0;
            CurrentSidewaysSleep = 0;
        }
    }

    /// <summary>
    ///     Update visual logic (Transform, FX).
    /// </summary>
    public void UpdateVisual()
    {
        UpdateTransform();

        /*if (WheelCollider.isGrounded && CurrentMaxSlip > SlipForGenerateParticle)
        {
            //Emit particle.
            var particles = FXController.GetAspahaltParticles;
            var point = WheelCollider.transform.position;
            point.y = Hit.point.y;
            particles.transform.position = point;
            particles.Emit (1);

            if (Trail == null)
            {
                //Get free or create trail.
                HitPoint = WheelCollider.transform.position;
                HitPoint.y = Hit.point.y;
                Trail = FXController.GetTrail (HitPoint);
                Trail.transform.SetParent (WheelCollider.transform);
                Trail.transform.localPosition += TrailOffset;
            }
        }
        else if (Trail != null)
        {
            //Set trail as free.
            FXController.SetFreeTrail (Trail);
            Trail = null;
        }*/
    }

    public void UpdateTransform()
    {
        Vector3 pos;
        Quaternion quat;
        WheelCollider.GetWorldPose(out pos, out quat);
        WheelView.position = pos;
        WheelView.rotation = quat;
    }

    public void UpdateFrictionConfig(PG_WheelColliderConfig config)
    {
        PG_WheelCollider.UpdateConfig(config);
    }
}
