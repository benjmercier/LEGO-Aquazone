using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LEGOAquazone.Scripts.AI.Flocking
{
    [System.Serializable]
    public class Flock
    {
        public Transform container;
        public Transform origin;
        public float originRadius = 10f;

        [Space]
        public List<GameObject> agentPrefabs = new List<GameObject>();
        public int agentNumber = 20;
        [Range(0f, 10f)]
        public float agentMinSpeed = 2f,
            agentMaxSpeed = 5f;

        public Flock(Transform container, Transform origin, float radius, List<GameObject> prefabs, int number, float minSpeed, float maxSpeed)
        {
            this.container = container;
            this.origin = origin;
            this.originRadius = radius;
            this.agentPrefabs = prefabs;
            this.agentNumber = number;
            this.agentMinSpeed = minSpeed;
            this.agentMaxSpeed = maxSpeed;
        }
    }

    public class FlockController : MonoBehaviour
    {
        [SerializeField]
        private Flock _flock;

        public float FlockRadius { get { return _flock.originRadius; } }

        public int ID;

        private Vector3 _randomSpawn;
        private Vector3 _spawnPos;
        private Quaternion _spawnRot;
        private GameObject _tempAgent;

        [SerializeField, Range(0f, 10f)]
        private float _flockMinSpeed = 2f,
            _flockMaxSpeed = 5f;

        [SerializeField]
        private List<FlockAgent> _activeFlockAgents = new List<FlockAgent>();

        [Header("Behavior Weights")]
        [SerializeField, Range(0f, 10f)]
        private float _cohesionWeight = 2f;
        public float CohesionWeight { get { return _cohesionWeight; } }
        [SerializeField, Range(0f, 10f)]
        private float _alignmentWeight = 2f;
        public float AlignmentWeight { get { return _alignmentWeight; } }
        [SerializeField, Range(0f, 10f)]
        private float _avoidanceWeight = 2f;
        public float AvoidanceWeight { get { return _avoidanceWeight; } }
        [SerializeField, Range(0f, 10f)]
        private float _containmentWeight = 2f;
        public float ContainmentWeight { get { return _containmentWeight; } }
        [SerializeField, Range(0f, 10f)]
        private float _obstacleWeight = 2f;
        public float ObstacleWeight { get { return _obstacleWeight; } }

        [Space]
        [SerializeField]
        private float _obstacleAvoidanceDist = 1f;
        public float ObstacleAvoidanceDist { get { return _obstacleAvoidanceDist; } }

        private void Start()
        {
            GenerateFlockAgents();
        }

        private void Update()
        {
            _activeFlockAgents.ForEach(agent => agent.MoveAgent());
        }

        private void GenerateFlockAgents()
        {
            foreach (var agent in _flock.agentPrefabs)
            {
                for (int i = 0; i < _flock.agentNumber; i++)
                {
                    _randomSpawn = Random.insideUnitSphere;
                    _randomSpawn *= _flock.originRadius;

                    _spawnPos = _flock.origin.position + _randomSpawn;
                    _spawnRot = Quaternion.Euler(0, Random.Range(0, 360), 0);

                    _tempAgent = Instantiate(agent, _spawnPos, _spawnRot);
                    _tempAgent.transform.parent = _flock.container;

                    if (_tempAgent.TryGetComponent(out FlockAgent flockAgent))
                    {
                        flockAgent.SetFlock(this);
                        flockAgent.SetSpeed(Random.Range(_flockMinSpeed, _flockMaxSpeed));
                        _activeFlockAgents.Add(flockAgent);
                    }
                }
            }
        }
    }
}

