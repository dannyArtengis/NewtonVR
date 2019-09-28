using Artengis;
using UnityEngine;
using UnityEngine.Events;

namespace NewtonVR
{
    public class NVRKnob : MonoBehaviour
    {
        public Rigidbody Rigidbody;

        [Tooltip("The (worldspace) rotation distance from the initial rotation you have to turn the knob for it to register as turned on")]
        public float RotationToActive = 90.0f;

        [Tooltip("Axis along which the knob can be rotated")]
        public Axis movementAxis = Axis.Z;

        [Tooltip("Is set to true when the button has been pressed down this update frame")]
        public bool KnobActivated = false;

        [Tooltip("Is set to true when the button has been released from the down position this update frame")]
        public bool KnobDeactivated = false;

        [Tooltip("Is set to true each frame the button is pressed down")]
        public bool KnobIsActive = false;

        [Tooltip("Is set to true if the button was in a pushed state last frame")]
        public bool KnobWasActive = false;

        [Tooltip("Invoked when the button is first activated")]
        public UnityEvent OnKnobActivated;

        [Tooltip("Invoked when the button was aktive and is released now")]
        public UnityEvent OnKnobDeactivated;

        protected Transform InitialPosition;
        protected float MinDistance = 0.001f;

        protected float PositionMagic = 1000f;

        protected float CurrentAngleDiff = 0.0f;

        private Vector3 InitialLocalPosition;
        private Vector3 ConstrainedPosition;

        private Quaternion InitialLocalRotation;
        private Quaternion ConstrainedRotation;

        private void Awake()
        {
            InitialPosition = new GameObject(string.Format("[{0}] Initial Position", this.gameObject.name)).transform;
            InitialPosition.parent = this.transform.parent;
            InitialPosition.localPosition = Vector3.zero;
            InitialPosition.localRotation = Quaternion.identity;

            if (Rigidbody == null)
                Rigidbody = this.GetComponent<Rigidbody>();

            if (Rigidbody == null)
            {
                Debug.LogError("There is no rigidbody attached to this button.");
            }

            InitialLocalPosition = this.transform.localPosition;
            ConstrainedPosition = InitialLocalPosition;

            InitialLocalRotation = this.transform.localRotation;
            ConstrainedRotation = InitialLocalRotation;
        }

        private void FixedUpdate()
        {
            ConstrainRotation();

            CurrentAngleDiff = Quaternion.Angle(InitialLocalRotation, this.transform.localRotation);
            Vector3 PositionDelta = InitialPosition.position - this.transform.position;
            this.Rigidbody.velocity = PositionDelta * PositionMagic * Time.deltaTime;
        }

        private void Update()
        {
            KnobWasActive = KnobIsActive;
            KnobIsActive = CurrentAngleDiff > RotationToActive;

            if (KnobWasActive == false && KnobIsActive == true)
            {
                KnobActivated = true;
                OnKnobActivated.Invoke();
            }
            else
                KnobActivated = false;

            if (KnobWasActive == true && KnobIsActive == false)
            {
                KnobDeactivated = true;
                OnKnobDeactivated.Invoke();
            }
            else
                KnobDeactivated = false;
        }

        private void ConstrainRotation()
        {
            Vector3 constrainedEuler = ConstrainedRotation.eulerAngles;
            Vector3 currentEuler = transform.localRotation.eulerAngles;

            if (movementAxis == Axis.X) constrainedEuler.x = currentEuler.x;
            if (movementAxis == Axis.Y) constrainedEuler.y = currentEuler.y;
            if (movementAxis == Axis.Z) constrainedEuler.z = currentEuler.z;

            this.transform.localRotation = Quaternion.Euler(constrainedEuler);
            this.transform.localPosition = ConstrainedPosition;
        }

        private void LateUpdate()
        {
            ConstrainRotation();
        }
    }
}