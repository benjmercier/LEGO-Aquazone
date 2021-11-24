using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LEGOAquazone.Scripts.AI.Steering.Behaviors;

namespace LEGOAquazone.Scripts.AI.Steering
{
    public class SteeringController : MonoBehaviour
    {
        [SerializeField]
        private float _maxSpeed = 10f;
        [SerializeField, Range(0f, 1f)]
        private float _maxForce = 0.25f;

        private Vector3 _desiredVelocity;
        private Vector3 _currentVelocity;

        private Vector3 _appliedLinearForce = Vector3.zero;
        private Vector3 _appliedAngularForce = Vector3.zero;

        private Rigidbody _rigidbody;

        [SerializeField]
        private Vector3 _target;

        [SerializeField]
        private bool _calculateArrival;
        [SerializeField]
        private float _approachRadius = 2.5f;
        private float _approachDistance;

        [Header("Seek")]
        [SerializeField]
        private bool _useSeek = false;
        [SerializeField]
        private Seek _seek;

        private void Start()
        {
            if (TryGetComponent(out Rigidbody rigidbody))
            {
                _rigidbody = rigidbody;
            }
        }

        private void FixedUpdate()
        {
            if (_target != null)
            {
                ApplyForce(CalculateSeek(_target));
            }
        }

        private Vector3 CalculateSeek(Vector3 target)
        {
            _desiredVelocity = target - transform.position;
            _approachDistance = _desiredVelocity.magnitude;
            _desiredVelocity.Normalize();

            if (_calculateArrival && CheckDistanceToTarget())
            {
                _desiredVelocity *= _approachDistance / _approachRadius * _maxSpeed;
            }
            else
            {
                _desiredVelocity *= _maxSpeed;
            }

            _desiredVelocity -= _currentVelocity;
            _desiredVelocity = Vector3.ClampMagnitude(_desiredVelocity, _maxForce);
            _desiredVelocity /= _rigidbody.mass;

            _currentVelocity += _desiredVelocity;
            _currentVelocity = Vector3.ClampMagnitude(_currentVelocity, _maxSpeed);

            return _currentVelocity;
        }

        private Vector3 CalculateFlee(Vector3 target)
        {
            return CalculateSeek(target) * -1f;
        }

        private bool CheckDistanceToTarget()
        {
            return _approachDistance <= _approachRadius;
        }

        private void ApplyForce(Vector3 currentVelocity)
        {
            transform.position += currentVelocity * Time.deltaTime;
            transform.forward += currentVelocity * Time.deltaTime;

            /*
            _appliedLinearForce = new Vector3(0f, _currentVelocity.y, _currentVelocity.z); //ApplyForce(new Vector3(0f, _pitch, _throttle), _linearForce);
            _appliedAngularForce = new Vector3(0f, _currentVelocity.x, 0f);

            _rigidbody.AddRelativeForce(_appliedLinearForce, ForceMode.Force);
            _rigidbody.AddRelativeTorque(_appliedAngularForce, ForceMode.Force);
            */
        }
    }
}

