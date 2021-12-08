using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LEGOAquazone.Scripts.Interfaces;
using LEGOAquazone.Scripts.AI.Flocking.Behaviors;

namespace LEGOAquazone.Scripts.AI.Flocking
{
    [SelectionBase]
    public class FlockAgent : MonoBehaviour, IAgent<FlockAgent>
    {
        public FlockAgent Agent { get { return this; } }
        private FlockController _currentFlock;
        public int FlockID { get { return _currentFlock.ID; } set { _currentFlock.ID = value; } }

        private Vector3 _cohesionVector,
            _alignmentVector,
            _avoidanceVector,
            _containmentVector,
            _obstacleVector;

        private Vector3 _currentVelocity;
        private Vector3 _moveVector;
        [SerializeField]
        private float _smoothDamp = 1f;
        private float _agentSpeed;
        
        [SerializeField]
        private Vector3[] _obstacleAvoidanceVectors;
        private Vector3 _currentAvoidanceVector;

        private ViewRadius _viewRadius;




        [SerializeField]
        private float _maxSpeed = 10f;
        [SerializeField, Range(0f, 1f)]
        private float _maxForce = 0.25f;

        private Vector3 _desiredVelocity;
        private Vector3 _currentSteerVelocity;


        private void Awake()
        {
            if (_viewRadius == null)
            {
                _viewRadius = gameObject.GetComponentInChildren<ViewRadius>();
            }
        }

        private void Update()
        {
            
        }

        public void SetFlock(FlockController currentFlock)
        {
            _currentFlock = currentFlock;
        }

        public void SetSpeed(float speed)
        {
            _agentSpeed = speed;
        }

        private void ApplyForces()
        {
            _cohesionVector = CalculateCohesion() * _currentFlock.CohesionWeight;
            _alignmentVector = CalculateAlignment() * _currentFlock.AlignmentWeight;
            _avoidanceVector = CalculateAvoidance() * _currentFlock.AvoidanceWeight;
            _containmentVector = CalculateContainment() * _currentFlock.ContainmentWeight;
            _obstacleVector = CalculateObstacle() * _currentFlock.ObstacleWeight;

            _moveVector = _cohesionVector + _avoidanceVector + _alignmentVector + _containmentVector + _obstacleVector;
        }

        public void MoveAgent()
        {
            MatchAvgNeighborSpeed();
            ApplyForces();


            _moveVector = Vector3.SmoothDamp(transform.forward, _moveVector, ref _currentVelocity, _smoothDamp);
            _moveVector = _moveVector.normalized * _agentSpeed;

            transform.position += _moveVector * Time.deltaTime;
            transform.forward += _moveVector;// * Time.deltaTime;
        }

        /*
        public void MoveAgent()
        {
            MatchAvgNeighborSpeed();
            ApplyForces();

            _moveVector = Vector3.SmoothDamp(transform.forward, _moveVector, ref _currentVelocity, _smoothDamp);
            _moveVector = _moveVector.normalized * _agentSpeed;

            transform.position += _moveVector * Time.deltaTime;
            transform.forward = _moveVector;
        }
        */

        private void MatchAvgNeighborSpeed()
        {
            if (_viewRadius.AgentsInFOV.Count > 0)
            {
                _agentSpeed = 0f;

                _viewRadius.AgentsInFOV.ForEach(obj =>
                {
                    if (obj.TryGetComponent(out FlockAgent agent))
                    {
                        _agentSpeed += agent._agentSpeed;
                    }
                });

                _agentSpeed /= _viewRadius.AgentsInFOV.Count;

                //_agentSpeed = Mathf.Clamp(_agentSpeed, 2f, 5f);

                //_agentSpeed *= UnityEngine.Random.Range(0.75f, 1.25f);
            }
        }

        private Vector3 CalculateCohesion()
        {
            _cohesionVector = Vector3.zero;

            if (_viewRadius.AgentsInFOV.Count > 0)
            {
                _viewRadius.AgentsInFOV.ForEach(agent => _cohesionVector += agent.transform.position);

                _cohesionVector /= _viewRadius.AgentsInFOV.Count;
                _cohesionVector -= transform.position;
                _cohesionVector.Normalize();
            }

            return _cohesionVector;
        }

        private Vector3 CalculateAlignment()
        {
            _alignmentVector = transform.forward;

            if (_viewRadius.AgentsInFOV.Count > 0)
            {
                _viewRadius.AgentsInFOV.ForEach(agent => _alignmentVector += agent.transform.forward);

                _alignmentVector /= _viewRadius.AgentsInFOV.Count;
                _alignmentVector.Normalize();
            }

            return _alignmentVector;
        }

        private Vector3 CalculateAvoidance()
        {
            _avoidanceVector = Vector3.zero;

            if (_viewRadius.AgentsInFOV.Count > 0)
            {
                _viewRadius.AgentsInFOV.ForEach(agent => _avoidanceVector += (transform.position - agent.transform.position));

                _avoidanceVector /= _viewRadius.AgentsInFOV.Count;
                _avoidanceVector.Normalize();
            }

            return _avoidanceVector;
        }

        private Vector3 CalculateContainment()
        {
            _containmentVector = _currentFlock.transform.position - transform.position;

            if (_containmentVector.magnitude >= _currentFlock.FlockRadius)
            {
                return _containmentVector.normalized;
            }

            return Vector3.zero;
        }

        private Vector3 CalculateObstacle()
        {
            _obstacleVector = Vector3.zero;

            if (_viewRadius.ObstaclesInFOV.Count > 0)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, transform.forward, out hit, _currentFlock.ObstacleAvoidanceDist))
                {
                    _obstacleVector = TurnToAvoidObstacle();
                }
                else
                {
                    _currentAvoidanceVector = Vector3.zero;
                }
            }

            return _obstacleVector;
        }

        private Vector3 TurnToAvoidObstacle()
        {
            if (_currentAvoidanceVector != Vector3.zero)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, _currentFlock.ObstacleAvoidanceDist))
                {
                    return _currentAvoidanceVector;
                }
            }

            float maxDistance = int.MinValue;
            var direction = Vector3.zero;

            for (int i = 0; i < _obstacleAvoidanceVectors.Length; i++)
            {
                RaycastHit hit;
                var newDirection = transform.TransformDirection(_obstacleAvoidanceVectors[i].normalized);

                if (Physics.Raycast(transform.position, newDirection, out hit, _currentFlock.ObstacleAvoidanceDist))
                {
                    float currentDistance = (hit.point - transform.position).sqrMagnitude;

                    if (currentDistance > maxDistance)
                    {
                        maxDistance = currentDistance;
                        direction = newDirection;
                    }
                }
                else
                {
                    direction = newDirection;
                    _currentAvoidanceVector = newDirection.normalized;

                    return direction.normalized;
                }
            }

            return direction.normalized;
        }
    }
}

