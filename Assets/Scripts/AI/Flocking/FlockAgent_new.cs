using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LEGOAquazone.Scripts.AI.Flocking
{
    public class BehaviorForce
    {
        public Vector3 vector;
        public int count;
    }

    public class FlockAgent_new : MonoBehaviour
    {
        [SerializeField]
        private float _maxSpeed = 10f;
        [SerializeField, Range(0f, 10f)]
        private float _maxForce = 0.25f;
        [SerializeField]
        private int _groupIndex;
        public int GroupIndex { get { return _groupIndex; } set { _groupIndex = value; } }

        private FlockController_new _flockController;
        public FlockController_new FlockController { get { return _flockController; } set { _flockController = value; } }

        private Transform _agentTransform;
        public Transform AgentTransform { get { return _agentTransform; } }

        [SerializeField]
        private float _minObjDistance = 1.5f,
            _maxObjDistance = 5f;

        private Vector3 _steeringTarget;
        private Vector3 _forceVector;
        private Vector3 _tempForce;

        [SerializeField]
        private BehaviorForce _cohesion = new BehaviorForce(),
            _alignment = new BehaviorForce(),
            _separation = new BehaviorForce(),
            _containment = new BehaviorForce(),
            _follow = new BehaviorForce(),
            _obstacleAvoidance = new BehaviorForce();

        private Vector3 _desiredVelocity,
            _currentVelocity;

        [SerializeField]
        private AgentEyeSight[] _agentEyes;
        private bool _inFOV = false;
        private Vector3 _nearbyAgentVector;

        private FlockAgent_new _leaderAgent;
        private float _leaderAngle = 180f;
        private float _tempAngle;

        private void Awake()
        {
            if (TryGetComponent(out Transform transform))
            {
                _agentTransform = transform;
            }

            if (_agentEyes.Length == 0)
            {
                _agentEyes = this.gameObject.GetComponentsInChildren<AgentEyeSight>();
            }
        }

        public void ApplyForces(List<FlockAgent_new> agents)
        {
            _steeringTarget = Vector3.zero;

            _steeringTarget = CalculateForces(agents);

            SteerAgent(_steeringTarget);
        }

        private Vector3 CalculateForces(List<FlockAgent_new> agents)
        {
            SetDefaultForce(_cohesion);
            SetDefaultForce(_alignment);
            SetDefaultForce(_separation);
            SetDefaultForce(_containment);
            SetDefaultForce(_follow);

            _leaderAgent = agents[0];

            foreach (var agent in agents)
            {
                if (agent == this)
                {
                    continue;
                }

                _inFOV = _agentEyes.Any(eye => eye.CheckIfInEyeFOV(agent.AgentTransform.position));

                if (_inFOV)
                {
                    _nearbyAgentVector = agent.AgentTransform.position - _agentTransform.position;

                    // cohesion & alignment
                    if (_nearbyAgentVector.magnitude < _maxObjDistance && agent.GroupIndex == this.GroupIndex)
                    {
                        AddForceVector(_cohesion, _nearbyAgentVector);

                        AddForceVector(_alignment, agent.AgentTransform.forward);

                        SetAgentToFollow(agent, _nearbyAgentVector);
                    }

                    // separation
                    if (_nearbyAgentVector.magnitude < _minObjDistance)
                    {
                        AddForceVector(_separation, _nearbyAgentVector);
                    }
                }
            }

            _cohesion.vector = AverageForceVector(_cohesion) - _agentTransform.position;
            _alignment.vector = AverageForceVector(_alignment);
            _separation.vector = AverageForceVector(_separation) * -1f;
            _containment.vector = CheckContainment(_flockController.Flock);
            _follow.vector = CheckLeadingAgent();

            _forceVector += _cohesion.vector * _flockController.CohesionWeight;// _cohesion.weight;
            _forceVector += _alignment.vector * _flockController.AlignmentWeight;// _alignment.weight;
            _forceVector += _separation.vector * _flockController.SeparationWeight;// _separation.weight;
            _forceVector += _containment.vector * _flockController.ContainmentWeight;// _containment.weight;
            _forceVector += _follow.vector * _flockController.FollowWeight;

            return _forceVector;
        }

        private void SetDefaultForce(BehaviorForce force)
        {
            force.vector = Vector3.zero;
            force.count = 0;
        }

        private void AddForceVector(BehaviorForce force, Vector3 vectorToAdd)
        {
            force.vector += vectorToAdd;
            force.count++;
        }

        private Vector3 AverageForceVector(BehaviorForce force)
        {
            _tempForce = force.vector;

            if (force.count > 0)
            {
                _tempForce /= force.count;
            }

            return _tempForce;
        }

        private void SetAgentToFollow(FlockAgent_new agent, Vector3 vectorToCheck)
        {
            _tempAngle = Vector3.Angle(vectorToCheck, _agentTransform.forward);

            if (_tempAngle < _leaderAngle && _tempAngle < 90f)
            {
                _leaderAgent = agent;
                _leaderAngle = _tempAngle;
            }
        }

        private Vector3 CheckLeadingAgent()
        {
            _tempForce = Vector3.zero;

            if (_leaderAgent != null)
            {
                _tempForce = _leaderAgent.AgentTransform.position - _agentTransform.position;
            }

            return _tempForce;
        }

        private Vector3 CheckContainment(Flock_new flock)
        {
            _tempForce = flock.spawnOrigin.position - _agentTransform.position;

            if (_tempForce.magnitude > flock.roamingRadius)
            {
                return _tempForce;
            }

            return Vector3.zero;
        }

        private void SteerAgent(Vector3 steeringTarget)
        {
            _desiredVelocity = (steeringTarget - _agentTransform.position).normalized;
            _desiredVelocity *= _maxSpeed;
            _desiredVelocity -= _currentVelocity;
            _desiredVelocity = Vector3.ClampMagnitude(_desiredVelocity, _maxForce);

            _currentVelocity += _desiredVelocity;
            _currentVelocity = Vector3.ClampMagnitude(_currentVelocity, _maxSpeed);
            _currentVelocity *= Time.deltaTime;
            
            _agentTransform.position += _currentVelocity;
            _agentTransform.forward += _currentVelocity;
        }
    }
}
