using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Autohand.Demo
{
    public class UnlockChest : MonoBehaviour
    {
        public PlacePoint placePoint;

        public HingeJoint joint;

        public UnityEvent OtherUnlockFunctions;

        public void Start()
        {
            placePoint.OnPlaceEvent += (PlacePoint point, Grabbable grab) => {
                Unlock();
                grab.body.detectCollisions = false;
            };
        }

        public void Unlock()
        {
            joint.limits = new JointLimits
            {
                bounceMinVelocity = joint.limits.bounceMinVelocity,
                bounciness = joint.limits.bounciness,
                contactDistance = joint.limits.contactDistance,
                min = 0,
                max = 160
            };
            joint.spring = new JointSpring() { spring = 5, targetPosition = 160 };

        }


        public void Lock()
        {
            joint.limits = new JointLimits
            {
                bounceMinVelocity = joint.limits.bounceMinVelocity,
                bounciness = joint.limits.bounciness,
                contactDistance = joint.limits.contactDistance,
                min = -2,
                max = 2
            };
        }

        public void CallOtherFunctions()
        {
            OtherUnlockFunctions.Invoke();
        }
    }
}