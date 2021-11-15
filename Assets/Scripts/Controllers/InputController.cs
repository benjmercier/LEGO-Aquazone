using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using LEGOAquazone.InputActions;

namespace LEGOAquazone.Scripts.Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class InputController : MonoBehaviour, PlayerInputActions.IPlayerActions
    {
        private Vector3 _moveInput;

        private Vector3 _moveDirection;

        //private PropellerController _propellerControl;

        // Events
        public static Action<Vector3> onMoveInput;
        public static Action<float> onBarrelRollInput;
        public static Action onFireInput;

        private void Update()
        {
            //_propellerControl.ControlSpeed(_currentVelocity.x, _currentVelocity.z);
        }

        private void FixedUpdate()
        {
            //CalculateSteering(_moveDirection);

            OnMoveInput(_moveDirection);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector3>();

            _moveDirection = new Vector3(_moveInput.x, _moveInput.z, _moveInput.y);
        }

        private void OnMoveInput(Vector3 direction)
        {
            onMoveInput?.Invoke(direction);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}

