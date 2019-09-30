using Artengis;
using UnityEngine;
using UnityEngine.Events;

namespace NewtonVR
{
    public class NVRButton : MonoBehaviour
    {
        public Rigidbody Rigidbody;

        [Tooltip("The (worldspace) distance from the initial position you have to push the button for it to register as pushed")]
        public float DistanceToEngage = 0.075f;

        [Tooltip( "Axis along which the button can be pushed" )]
        public Axis movementAxis = Axis.Y;

        [Tooltip("Is set to true when the button has been pressed down this update frame")]
        public bool ButtonDown = false;

        [Tooltip("Is set to true when the button has been released from the down position this update frame")]
        public bool ButtonUp = false;

        [Tooltip("Is set to true each frame the button is pressed down")]
        public bool ButtonIsPushed = false;

        [Tooltip("Is set to true if the button was in a pushed state last frame")]
        public bool ButtonWasPushed = false;

        [Tooltip("Invoked when the button is first activated")]
        public UnityEvent OnButtonDown;

        [Tooltip("Invoked when the button was aktive and is released now")]
        public UnityEvent OnButtonUp;

        protected Transform InitialPosition;
        protected float MinDistance = 0.001f;

        protected float PositionMagic = 1000f;

        protected float CurrentDistance = -1;

        private Vector3 InitialLocalPosition;
        private Vector3 ConstrainedPosition;
        private Vector3 MinPositionVec;

        private Quaternion InitialLocalRotation;
        private Quaternion ConstrainedRotation;

        private void Awake()
        {
            InitialPosition = new GameObject(string.Format("[{0}] Initial Position", this.gameObject.name)).transform;
            InitialPosition.parent = this.transform.parent;
            InitialPosition.localPosition = Vector3.zero;
            InitialPosition.localRotation = Quaternion.identity;

            MinPositionVec = InitialPosition.localPosition - new Vector3(DistanceToEngage, DistanceToEngage, DistanceToEngage);

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
            ConstrainPosition();

            CurrentDistance = Vector3.Distance(this.transform.position, InitialPosition.position);

            Vector3 PositionDelta = InitialPosition.position - this.transform.position;
            this.Rigidbody.velocity = PositionDelta * PositionMagic * Time.deltaTime;
        }

        private void Update()
        {
            ButtonWasPushed = ButtonIsPushed;
            ButtonIsPushed = CurrentDistance > DistanceToEngage;

            if (ButtonWasPushed == false && ButtonIsPushed == true)
            {
                ButtonDown = true;
                OnButtonDown.Invoke();
            }
            else
                ButtonDown = false;

            if (ButtonWasPushed == true && ButtonIsPushed == false)
            {
                ButtonUp = true;
                OnButtonUp.Invoke();
            }
            else
                ButtonUp = false;
        }

        private void ConstrainPosition()
        {
            if (movementAxis == Axis.X) ConstrainedPosition.x = Mathf.Clamp(transform.localPosition.x, MinPositionVec.x, 0.0f);
            else if (movementAxis == Axis.Y) ConstrainedPosition.y = Mathf.Clamp(transform.localPosition.y, MinPositionVec.y, 0.0f);
            else if (movementAxis == Axis.Z) ConstrainedPosition.z = Mathf.Clamp(transform.localPosition.z, MinPositionVec.z, 0.0f);

            transform.localPosition = ConstrainedPosition;
            transform.localRotation = ConstrainedRotation;
        }

        private void LateUpdate()
        {
            ConstrainPosition();
        }
    }
}