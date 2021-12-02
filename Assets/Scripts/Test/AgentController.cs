using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LEGOAquazone.Scripts.Test
{
    public class AgentController : MonoBehaviour
    {
        [SerializeField]
        private float _maxSpeed = 5f;
        [SerializeField, Range(0f, 1f)]
        private float _maxForce = 0.25f;

        [SerializeField]
        private bool _isPrey = false;

        [SerializeField]
        private GameObject _targetObj;
        private AgentController _targetController;

        private Vector3 _desiredVelocity;
        private Vector3 _currentVelocity;
        public Vector3 CurrentVelocity { get { return _currentVelocity; } }

        [SerializeField]
        private float _pursuit = 1.5f;
        [SerializeField]
        private float _evade = 1.5f;

        [SerializeField]
        private bool _calculateArrival;
        [SerializeField]
        private float _approachRadius = 2.5f;
        private float _approachDistance;

        private Vector3 _appliedLinearForce = Vector3.zero;
        private Vector3 _appliedAngularForce = Vector3.zero;

        private Rigidbody _rigidbody;

        // Wander
        private Vector3 _circleCenter;
        private float _circleDistance = 2f;
        private float _circleRadius = 1;
        private float _wanderTheta = Mathf.PI / 2;
        private Vector3 _wanderForce;
        private float _maxRadius = 15f;
        private Vector3 _wanderTarget = Vector3.zero;
        private float _turnChance = 0.05f;


        private void Start()
        {
            if (TryGetComponent(out Rigidbody rigidbody))
            {
                _rigidbody = rigidbody;
            }

            //_targetVector = _targetObj.transform.position;

            if (_targetObj.TryGetComponent(out AgentController agentController))
            {
                _targetController = agentController;
            }

            _currentVelocity = Random.onUnitSphere;
            _wanderForce = CalculateWander(_currentVelocity);
        }

        private void FixedUpdate()
        {
            //ApplyForce(CalculateArrival(_targetObj.transform.position));

            ApplyForce(CalculateSeek(_currentVelocity, 1));
            
            /*
            if (_isPrey)
            {
                ApplyForce(CalculateEvade(_targetObj.transform.position, _targetController.CurrentVelocity));
            }
            else
            {
                ApplyForce(CalculatePursuit(_targetObj.transform.position, _targetController.CurrentVelocity));
            }
            */
        }

        // Single Target
        private Vector3 CalculateSeek(Vector3 target)
        {
            _desiredVelocity = (target - transform.position).normalized;
            _desiredVelocity *= _maxSpeed;
            _desiredVelocity -= _currentVelocity;
            _desiredVelocity = Vector3.ClampMagnitude(_desiredVelocity, _maxForce);
            _desiredVelocity /= _rigidbody.mass;

            _currentVelocity += _desiredVelocity;
            _currentVelocity = Vector3.ClampMagnitude(_currentVelocity, _maxSpeed);

            return _currentVelocity;
        }

        private Vector3 CalculateSeek(Vector3 target, bool useArrival = false)
        {
            _desiredVelocity = target - transform.position;
            _approachDistance = _desiredVelocity.magnitude;
            _desiredVelocity.Normalize();

            if (_calculateArrival && TargetWithinApproachRadius())
            {
                _desiredVelocity *= _approachDistance / _approachRadius * _maxSpeed;

                //GetNewTarget();
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

        private Vector3 CalculateSeek(Vector3 target, int wander = 1) // with wander
        {
            _desiredVelocity = GetWanderForce().normalized;
            _desiredVelocity *= _maxSpeed;
            _desiredVelocity -= _currentVelocity;
            _desiredVelocity = Vector3.ClampMagnitude(_desiredVelocity, _maxForce);
            _desiredVelocity /= _rigidbody.mass;

            _currentVelocity += _desiredVelocity;
            _currentVelocity = Vector3.ClampMagnitude(_currentVelocity, _maxSpeed);

            return _currentVelocity;
        }

        private bool TargetWithinApproachRadius()
        {
            return _approachDistance <= _approachRadius;
        }

        private Vector3 CalculateFlee(Vector3 target)
        {
            return CalculateSeek(target) * -1f;
        }

        private Vector3 CalculatePursuit(Vector3 target, Vector3 targetVelocity)
        {
            var prediction = target + targetVelocity;
            prediction *= _pursuit;

            return CalculateSeek(prediction);
        }

        private Vector3 CalculateEvade(Vector3 target, Vector3 targetVelocity)
        {
            return CalculatePursuit(target, targetVelocity) * -1f;
        }

        private Vector3 CalculateArrival(Vector3 target)
        {
            return CalculateSeek(target, _calculateArrival);
        }

        private Vector3 GetWanderForce()
        {
            if (transform.position.magnitude > _maxRadius)
            {
                var directionToCenter = (_wanderTarget - transform.position).normalized;
                _wanderForce = _currentVelocity.normalized + directionToCenter;
            }
            else if (Random.value < _turnChance)
            {
                _wanderForce = CalculateWander(_currentVelocity);
            }

            return _wanderForce;
        }

        private Vector3 CalculateWander(Vector3 velocity)
        {
            _circleCenter = _currentVelocity.normalized;
            var random = Random.insideUnitCircle;

            var displacement = new Vector3(random.x, random.y) * _circleRadius;
            displacement = Quaternion.LookRotation(_currentVelocity) * displacement;

            var wanderForce = _circleCenter + displacement;

            return wanderForce;

            /*
            _circleCenter = velocity;
            _circleCenter.Normalize();

            var circle = transform.position + (_circleCenter * _circleDistance);

            Debug.Log("Agent: " + transform.position + " \n Circle: " + circle + " \n Center: " + _circleCenter);

            var displacement = new Vector3(0, -1f);

            return circle;
            */
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

