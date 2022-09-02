using System;
using UnityEngine;

namespace AllIn1VfxToolkit
{
    public class AllIn1LookAt : MonoBehaviour
    {
        //Otherwise we just update on Start
        [SerializeField]
        bool updateEveryFrame;

        [Space]
        [Header("Choose Target")]
        [SerializeField]
        bool targetIsMainCamera;
        [SerializeField]
        Transform target;

        enum FaceDirection
        {
            Forward,
            Up,
            Right
        };

        [Space]
        [Header("Look At Direction")]
        [SerializeField]
        FaceDirection faceDirection;
        [SerializeField]
        bool negateDirection;

        void Start()
        {
            if (targetIsMainCamera)
            {
                if (!(Camera.main is null)) target = Camera.main.transform;

                if (target == null)
                {
                    Debug.LogError("No main camera was found, AllIn1LookAt component of " + gameObject.name +
                                   " will now be destroyed. Please double check your setup");

                    Destroy(this);
                }
            }
            else
            {
                if (target == null)
                {
                    Debug.LogError("No target was assigned, AllIn1LookAt component of " + gameObject.name +
                                   " will now be destroyed. Please double check your setup");

                    Destroy(this);
                }
            }

            if (!updateEveryFrame) LookAtCompute();
        }

        void Update()
        {
            if (updateEveryFrame) LookAtCompute();
        }

        void LookAtCompute()
        {
            Vector3 lookAtVector = (target.position - transform.position).normalized;
            if (negateDirection) lookAtVector = -lookAtVector;

            switch (faceDirection)
            {
                case FaceDirection.Forward:
                    transform.forward = lookAtVector;
                    break;
                case FaceDirection.Up:
                    transform.up = lookAtVector;
                    break;
                case FaceDirection.Right:
                    transform.right = lookAtVector;
                    break;
            }
        }
    }
}
