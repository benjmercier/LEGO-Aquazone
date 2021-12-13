using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LEGOAquazone.Scripts.AI.Flocking
{
    [SelectionBase]
    public class FlockController_new : MonoBehaviour
    {
        [SerializeField]
        private Flock_new _flock;
        public Flock_new Flock { get { return _flock; } }

        // Spawning
        private GameObject _tempAgent;
        private Vector3 _spawnPos;
        private Quaternion _spawnRot;
        private int _randomIndex;

        [Header("Behavior Weights")]
        [SerializeField, Range(0f, 10f)]
        private float _cohesionWeight = 2f;
        [SerializeField, Range(0f, 10f)]
        private float _alignmentWeight = 2f;
        [SerializeField, Range(0f, 10f)]
        private float _separationWeight = 2f;
        [SerializeField, Range(0f, 10f)]
        private float _containmentWeight = 2f;
        [SerializeField, Range(0f, 10f)]
        private float _followWeight = 2f;
        [SerializeField, Range(0f, 10f)]
        private float _obstacleAvoidanceWeight = 2f;

        public float CohesionWeight { get { return _cohesionWeight; } }
        public float AlignmentWeight { get { return _alignmentWeight; } }
        public float SeparationWeight { get { return _separationWeight; } }
        public float ContainmentWeight { get { return _containmentWeight; } }
        public float FollowWeight { get { return _followWeight; } }
        public float ObstacleAvoidanceWeight { get { return _obstacleAvoidanceWeight; } }

        private void Start()
        {
            GenerateAgents();   
        }

        private void Update()
        {
            MoveAgents();
        }

        // eventually switch to obj pooling
        private void GenerateAgents()
        {
            for (int i = 0; i < _flock.totalAgents; i++)
            {
                _randomIndex = UnityEngine.Random.Range(0, _flock.agentPrefabs.Count);

                _spawnPos = Random.insideUnitSphere;
                _spawnPos *= _flock.roamingRadius;
                _spawnPos += _flock.spawnOrigin.position;

                _spawnRot = Quaternion.Euler(UnityEngine.Random.Range(0f, 90f), UnityEngine.Random.Range(0f, 360f), 0f);

                _tempAgent = Instantiate(_flock.agentPrefabs[_randomIndex].gameObject, _spawnPos, _spawnRot);
                _tempAgent.transform.parent = _flock.spawnContainer.transform;

                if (_tempAgent.TryGetComponent(out FlockAgent_new flockAgent))
                {
                    flockAgent.GroupIndex = UnityEngine.Random.Range(0, _flock.totalGroups);
                    flockAgent.FlockController = this;

                    _flock.activeAgents.Add(flockAgent);
                }
            }
        }

        private void MoveAgents()
        {
            foreach (var agent in _flock.activeAgents)
            {
                // move
                agent.ApplyForces(_flock.activeAgents);
            }
        }
    }
}
