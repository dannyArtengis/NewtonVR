﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

#if NVR_Oculus
namespace NewtonVR
{
    public class NVROculusInputDevice : NVRInputDevice
    {
        private OVRInput.Controller Controller;

        private Dictionary<NVRButtons, OVRInput.Button> ButtonMapping = new Dictionary<NVRButtons, OVRInput.Button>(new EnumEqualityComparer<NVRButtons>());
        private Dictionary<NVRButtons, OVRInput.Touch> TouchMapping = new Dictionary<NVRButtons, OVRInput.Touch>(new EnumEqualityComparer<NVRButtons>());
        private Dictionary<NVRButtons, OVRInput.NearTouch> NearTouchMapping = new Dictionary<NVRButtons, OVRInput.NearTouch>(new EnumEqualityComparer<NVRButtons>());
        private Dictionary<NVRButtons, OVRInput.Axis1D> TriggerMapping = new Dictionary<NVRButtons, OVRInput.Axis1D>(new EnumEqualityComparer<NVRButtons>());
        private Dictionary<NVRButtons, OVRInput.Axis2D> StickMapping = new Dictionary<NVRButtons, OVRInput.Axis2D>(new EnumEqualityComparer<NVRButtons>());

        public override void Initialize(NVRHand hand)
        {
            base.Initialize(hand);

            SetupButtonMapping();

            if (hand == Hand.Player.LeftHand)
            {
                Controller = OVRInput.Controller.LTouch;
            }
            else
            {
                Controller = OVRInput.Controller.RTouch;
            }
            
        }

        protected virtual void SetupButtonMapping()
        {
            ButtonMapping.Add(NVRButtons.A, OVRInput.Button.One);
            ButtonMapping.Add(NVRButtons.B, OVRInput.Button.Two);
            ButtonMapping.Add(NVRButtons.X, OVRInput.Button.One);
            ButtonMapping.Add(NVRButtons.Y, OVRInput.Button.Two);
            ButtonMapping.Add(NVRButtons.Touchpad, OVRInput.Button.PrimaryThumbstick);
            ButtonMapping.Add(NVRButtons.DPad_Up, OVRInput.Button.DpadUp);
            ButtonMapping.Add(NVRButtons.DPad_Down, OVRInput.Button.DpadDown);
            ButtonMapping.Add(NVRButtons.DPad_Left, OVRInput.Button.DpadLeft);
            ButtonMapping.Add(NVRButtons.DPad_Right, OVRInput.Button.DpadRight);
            ButtonMapping.Add(NVRButtons.Trigger, OVRInput.Button.PrimaryIndexTrigger);
            ButtonMapping.Add(NVRButtons.Grip, OVRInput.Button.PrimaryHandTrigger);
            ButtonMapping.Add(NVRButtons.System, OVRInput.Button.Back);

            TouchMapping.Add(NVRButtons.A, OVRInput.Touch.One);
            TouchMapping.Add(NVRButtons.B, OVRInput.Touch.Two);
            TouchMapping.Add(NVRButtons.X, OVRInput.Touch.One);
            TouchMapping.Add(NVRButtons.Y, OVRInput.Touch.Two);
            TouchMapping.Add(NVRButtons.Touchpad, OVRInput.Touch.PrimaryThumbstick);
            TouchMapping.Add(NVRButtons.Trigger, OVRInput.Touch.PrimaryIndexTrigger);
            
            NearTouchMapping.Add(NVRButtons.Touchpad, OVRInput.NearTouch.PrimaryThumbButtons);
            NearTouchMapping.Add(NVRButtons.Trigger, OVRInput.NearTouch.PrimaryIndexTrigger);

            TriggerMapping.Add(NVRButtons.Grip, OVRInput.Axis1D.PrimaryHandTrigger);
            TriggerMapping.Add(NVRButtons.Trigger, OVRInput.Axis1D.PrimaryIndexTrigger);

            StickMapping.Add(NVRButtons.Touchpad, OVRInput.Axis2D.PrimaryThumbstick);
        }

        private OVRInput.Button GetButtonMap(NVRButtons button)
        {
            if (ButtonMapping.ContainsKey(button) == false)
            {
                //Debug.LogError("No Oculus button configured for: " + button.ToString());
                return OVRInput.Button.None;
            }
            return ButtonMapping[button];
        }

        private OVRInput.Touch GetTouchMap(NVRButtons button)
        {
            if (TouchMapping.ContainsKey(button) == false)
            {
                //Debug.LogError("No Oculus touch map configured for: " + button.ToString());
                return OVRInput.Touch.None;
            }
            return TouchMapping[button];
        }

        private OVRInput.NearTouch GetNearTouchMap(NVRButtons button)
        {
            if (NearTouchMapping.ContainsKey(button) == false)
            {
                //Debug.LogError("No Oculus near touch map configured for: " + button.ToString());
                return OVRInput.NearTouch.None;
            }
            return NearTouchMapping[button];
        }

        private OVRInput.Axis1D GetTriggerMap(NVRButtons button)
        {
            if (TriggerMapping.ContainsKey(button) == false)
            {
                //Debug.LogError("No Oculus trigger map configured for: " + button.ToString());
                return OVRInput.Axis1D.None;
            }
            return TriggerMapping[button];
        }

        private OVRInput.Axis2D GetStickMap(NVRButtons button)
        {
            if (StickMapping.ContainsKey(button) == false)
            {
                //Debug.LogError("No Oculus stick map configured for: " + button.ToString());
                return OVRInput.Axis2D.None;
            }
            return StickMapping[button];
        }

        public override float GetAxis1D(NVRButtons button)
        {
            if (OVRInput.GetActiveController() == OVRInput.Controller.Touch)
                return OVRInput.Get(GetTriggerMap(button), Controller);

            return 0;
        }

        public override Vector2 GetAxis2D(NVRButtons button)
        {
            if (OVRInput.GetActiveController() == OVRInput.Controller.Touch)
                return OVRInput.Get(GetStickMap(button), Controller);

            return Vector2.zero;
        }

        public override bool GetPressDown(NVRButtons button)
        {
            if (OVRInput.GetActiveController() == OVRInput.Controller.Touch)
                return OVRInput.GetDown(GetButtonMap(button), Controller);

            return false;
        }

        public override bool GetPressUp(NVRButtons button)
        {
            if (OVRInput.GetActiveController() == OVRInput.Controller.Touch)
                return OVRInput.GetUp(GetButtonMap(button), Controller);

            return false;
        }

        public override bool GetPress(NVRButtons button)
        {
            if (OVRInput.GetActiveController() == OVRInput.Controller.Touch)
                return OVRInput.Get(GetButtonMap(button), Controller);

            return false;
        }

        public override bool GetTouchDown(NVRButtons button)
        {
            if (OVRInput.GetActiveController() == OVRInput.Controller.Touch)
                return OVRInput.GetDown(GetTouchMap(button), Controller);

            return false;
        }

        public override bool GetTouchUp(NVRButtons button)
        {
            if (OVRInput.GetActiveController() == OVRInput.Controller.Touch)
                return OVRInput.GetUp(GetTouchMap(button), Controller);

            return false;
        }

        public override bool GetTouch(NVRButtons button)
        {
            if (OVRInput.GetActiveController() == OVRInput.Controller.Touch)
                return OVRInput.Get(GetTouchMap(button), Controller);

            return false;
        }

        public override bool GetNearTouchDown(NVRButtons button)
        {
            if (OVRInput.GetActiveController() == OVRInput.Controller.Touch)
                return OVRInput.GetDown(GetNearTouchMap(button), Controller);

            return false;
        }

        public override bool GetNearTouchUp(NVRButtons button)
        {
            if (OVRInput.GetActiveController() == OVRInput.Controller.Touch)
                return OVRInput.GetUp(GetNearTouchMap(button), Controller);

            return false;
        }

        public override bool GetNearTouch(NVRButtons button)
        {
            if (OVRInput.GetActiveController() == OVRInput.Controller.Touch)
                return OVRInput.Get(GetNearTouchMap(button), Controller);

            return false;
        }

        public override void TriggerHapticPulse(ushort durationMicroSec = 500, NVRButtons button = NVRButtons.Touchpad)
        {
            if (OVRInput.GetActiveController() == OVRInput.Controller.Touch)
            {
                StartCoroutine(DoHapticPulse(durationMicroSec));
            }
        }

        private IEnumerator DoHapticPulse(ushort durationMicroSec)
        {
            OVRInput.SetControllerVibration(0.2f, 0.2f, Controller);    //Should we allow setting strength
            float endTime = Time.time + ((float)durationMicroSec / 1000000);
            do
            {
                yield return null;
            } while (Time.time < endTime);
            OVRInput.SetControllerVibration(0, 0, Controller);
        }

        public override bool IsCurrentlyTracked
        {
            get
            {
                return OVRInput.GetActiveController() == OVRInput.Controller.Touch;
            }
        }


        public override GameObject SetupDefaultRenderModel()
        {
            GameObject renderModel;

            if (Hand.IsLeft == true)
            {
                renderModel = GameObject.Instantiate(Resources.Load<GameObject>("TouchControllers/oculusTouchLeft"));
            }
            else
            {
                renderModel = GameObject.Instantiate(Resources.Load<GameObject>("TouchControllers/oculusTouchRight"));
            }

            renderModel.name = "Render Model for " + Hand.gameObject.name;
            renderModel.transform.parent = Hand.transform;
            renderModel.transform.localPosition = Vector3.zero;
            renderModel.transform.localRotation = Quaternion.identity;
            renderModel.transform.localScale = Vector3.one;

            return renderModel;
        }

        public override bool ReadyToInitialize()
        {
            return true;
        }

        public override string GetDeviceName()
        {
            if (Hand.HasCustomModel == true)
            {
                return "Custom";
            }
            else
            {
                return OVRInput.GetActiveController().ToString();
            }
        }

        public override Collider[] SetupDefaultPhysicalColliders(Transform ModelParent)
        {
            Collider[] Colliders = null;

            string name = "oculusTouch";
            if (Hand.IsLeft == true)
            {
                name += "Left";
            }
            else
            {
                name += "Right";
            }
            name += "Colliders";

            Transform touchColliders = ModelParent.transform.FindChild(name);
            if (touchColliders == null)
            {
                touchColliders = GameObject.Instantiate(Resources.Load<GameObject>("TouchControllers/" + name)).transform;
                touchColliders.parent = ModelParent.transform;
                touchColliders.localPosition = Vector3.zero;
                touchColliders.localRotation = Quaternion.identity;
                touchColliders.localScale = Vector3.one;
            }

            Colliders = touchColliders.GetComponentsInChildren<Collider>();

            return Colliders;
        }

        public override Collider[] SetupDefaultColliders()
        {
            Collider[] Colliders = null;

            //string controllerModel = GetDeviceName();

            SphereCollider OculusCollider = this.gameObject.AddComponent<SphereCollider>();
            OculusCollider.isTrigger = true;
            OculusCollider.radius = 0.15f;

            Colliders = new Collider[] { OculusCollider };

            return Colliders;
        }
        
    }
}
#else
namespace NewtonVR
{
    public class NVROculusInputDevice : NVRInputDevice
    {
        public override bool IsCurrentlyTracked
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override float GetAxis1D(NVRButtons button)
        {
            throw new NotImplementedException();
        }

        public override Vector2 GetAxis2D(NVRButtons button)
        {
            throw new NotImplementedException();
        }

        public override string GetDeviceName()
        {
            throw new NotImplementedException();
        }

        public override bool GetNearTouch(NVRButtons button)
        {
            throw new NotImplementedException();
        }

        public override bool GetNearTouchDown(NVRButtons button)
        {
            throw new NotImplementedException();
        }

        public override bool GetNearTouchUp(NVRButtons button)
        {
            throw new NotImplementedException();
        }

        public override bool GetPress(NVRButtons button)
        {
            throw new NotImplementedException();
        }

        public override bool GetPressDown(NVRButtons button)
        {
            throw new NotImplementedException();
        }

        public override bool GetPressUp(NVRButtons button)
        {
            throw new NotImplementedException();
        }

        public override bool GetTouch(NVRButtons button)
        {
            throw new NotImplementedException();
        }

        public override bool GetTouchDown(NVRButtons button)
        {
            throw new NotImplementedException();
        }

        public override bool GetTouchUp(NVRButtons button)
        {
            throw new NotImplementedException();
        }

        public override bool ReadyToInitialize()
        {
            throw new NotImplementedException();
        }

        public override Collider[] SetupDefaultColliders()
        {
            throw new NotImplementedException();
        }

        public override Collider[] SetupDefaultPhysicalColliders(Transform ModelParent)
        {
            throw new NotImplementedException();
        }

        public override GameObject SetupDefaultRenderModel()
        {
            throw new NotImplementedException();
        }

        public override void TriggerHapticPulse(ushort durationMicroSec = 500, NVRButtons button = NVRButtons.Touchpad)
        {
            throw new NotImplementedException();
        }
    }
}
#endif
