using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LEGOAquazone.Scripts.Controllers;

namespace LEGOAquazone.Scripts.Player.Movement
{
    public class Steering : MonoBehaviour
    {
        private Vector3 _targetDirection;

        [SerializeField]
        private float _maxSpeed = 35f;
        [SerializeField, Range(0f, 5f)]
        private float _maxForce = 2f;

        private Vector3 _desiredVelocity;
        private Vector3 _steeringVelocity;
        private Vector3 _currentVelocity;

        [SerializeField]
        private Rigidbody _rigidbody;

        private Vector3 _appliedLinearForce = Vector3.zero;
        private Vector3 _appliedAngularForce = Vector3.zero;

        // _moveDirection
        private float _pitch;
        private float _yaw;
        private float _roll;
        private float _strafe;
        private float _throttle;

        private float _throttleSpeed = 0.5f;

        public static Action<Transform, Vector3> onGetPlayerCurrentVelocity;

        private void Start()
        {
            if (_rigidbody == null)
            {
                if (TryGetComponent(out Rigidbody rigidbody))
                {
                    _rigidbody = rigidbody;
                }
                else
                {
                    Debug.Log("Steering::Start()::_rigidbody is NULL on " + gameObject.transform.name);
                }
            }
        }

        private void OnEnable()
        {
            InputController.onMoveInput += CalculateSteering;
        }

        private void OnDisable()
        {
            InputController.onMoveInput -= CalculateSteering;
        }

        private void Update()
        {
            OnGetPlayerCurrentVelocity(this.transform, _currentVelocity);
        }

        private void CalculateSteering(Vector3 direction)
        {
            //
            _pitch = direction.y;
            _yaw = direction.x;
            _throttle = direction.z;
            //

            _targetDirection = transform.position + direction;

            _desiredVelocity = (_targetDirection - transform.position).normalized;
            _desiredVelocity *= _maxSpeed;

            _steeringVelocity = _desiredVelocity - _currentVelocity;
            _steeringVelocity = Vector3.ClampMagnitude(_steeringVelocity, _maxForce);
            _steeringVelocity /= _rigidbody.mass;

            _currentVelocity += _steeringVelocity;
            _currentVelocity = Vector3.ClampMagnitude(_currentVelocity, _maxSpeed);

            _appliedLinearForce = new Vector3(0f, _currentVelocity.y, _currentVelocity.z); //ApplyForce(new Vector3(0f, _pitch, _throttle), _linearForce);
            _appliedAngularForce = new Vector3(0f, _currentVelocity.x, 0f);

            if (_currentVelocity.z < 0)
            {
                _appliedAngularForce *= -1f;
            }

            _rigidbody.AddRelativeForce(_appliedLinearForce, ForceMode.Force);
            _rigidbody.AddRelativeTorque(_appliedAngularForce, ForceMode.Force);

            
        }

        private void OnGetPlayerCurrentVelocity(Transform rootObj, Vector3 currentVelocity)
        {
            onGetPlayerCurrentVelocity?.Invoke(rootObj, currentVelocity);
        }
    }
}
