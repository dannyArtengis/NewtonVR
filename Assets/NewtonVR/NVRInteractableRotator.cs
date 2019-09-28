using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace NewtonVR
{
    public class NVRInteractableRotator : NVRInteractable
    {
        public float CurrentAngle;

        protected virtual float DeltaMagic { get { return 1f; } }
        protected Transform InitialAttachPoint;

        public UnityEvent OnBeginInteraction;
        public UnityEvent OnEndInteraction;

        protected override void Awake()
        {
            base.Awake();
            this.Rigidbody.maxAngularVelocity = 100f;
        }

        protected virtual void FixedUpdate()
        {
            if (IsAttached == true)
            {
                Vector3 PositionDelta = (AttachedHand.transform.position - InitialAttachPoint.position) * DeltaMagic;

                this.Rigidbody.AddForceAtPosition(PositionDelta, InitialAttachPoint.position, ForceMode.VelocityChange);
            }

            CurrentAngle = Quaternion.Angle(Quaternion.identity, this.transform.rotation);
        }

        public override void BeginInteraction(NVRHand hand)
        {
            base.BeginInteraction(hand);

            InitialAttachPoint = new GameObject(string.Format("[{0}] InitialAttachPoint", this.gameObject.name)).transform;
            InitialAttachPoint.position = hand.transform.position;
            InitialAttachPoint.rotation = hand.transform.rotation;
            InitialAttachPoint.localScale = Vector3.one * 0.25f;
            InitialAttachPoint.parent = this.transform;

            if (OnBeginInteraction != null)
            {
                OnBeginInteraction.Invoke();
            }
        }

        public override void EndInteraction(NVRHand hand)
        {
            base.EndInteraction(hand);

            if (InitialAttachPoint != null)
                Destroy(InitialAttachPoint.gameObject);

            if (OnEndInteraction != null)
            {
                OnEndInteraction.Invoke();
            }
        }

    }
}