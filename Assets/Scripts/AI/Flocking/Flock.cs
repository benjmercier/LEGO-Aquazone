using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LEGOAquazone.Scripts.ScriptableObjects;

namespace LEGOAquazone.Scripts.AI.Flocking
{
    public class Flock : MonoBehaviour
    {
        [SerializeField]
        private FlockAgent _agentPrefab;
        private List<FlockAgent> _agentList = new List<FlockAgent>();
        [SerializeField]
        private FlockBehavior _flockBehavior;

        [SerializeField, Range(5, 100)]
        private int _flockSize = 50;

        private const float _agentDensity = 0.08f;

        [SerializeField, Range(1f, 100f)]
        private float _driveFactor = 10f;
        [SerializeField, Range(1f, 100f)]
        private float _maxSpeed = 5f;
        [SerializeField, Range(1f, 10f)]
        private float _neighborRadius = 1.5f;
        [SerializeField, Range(0f, 1f)]
        private float _avoidanceRadiusMultiplier = 0.5f;

        private float _sqrMaxSpeed;
        private float _sqrNeighborRadius;
        private float _sqrAvoidanceRadius;
        public float SqrAvoidanceRadius { get { return _sqrAvoidanceRadius; } }

        private void Start()
        {
            _sqrMaxSpeed = _maxSpeed * _maxSpeed;
            _sqrNeighborRadius = _neighborRadius * _neighborRadius;
            _sqrAvoidanceRadius = _sqrNeighborRadius * _avoidanceRadiusMultiplier * _avoidanceRadiusMultiplier;

            for (int i = 0; i < _flockSize; i++)
            {
                FlockAgent agent = Instantiate(_agentPrefab,
                    Random.insideUnitSphere * _flockSize * _agentDensity,
                    Quaternion.Euler(Vector3.up * Random.Range(0, 360)), // sets random rotation on Y-axis
                    this.transform
                    );

                agent.name = "Agent " + i;
                _agentList.Add(agent);
            }
        }

        private void Update()
        {
            foreach (var agent in _agentList)
            {
                List<Transform> context = GetNearbyAgents(agent);

                // Testing
                //agent.GetComponentInChildren<Renderer>().material.color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

                Vector3 move = _flockBehavior.CalculateMove(agent, context, this);
                move *= _driveFactor;

                if (move.sqrMagnitude > _sqrMaxSpeed)
                {
                    move = move.normalized * _maxSpeed;
                }

                agent.MoveAgent(move);
            }
        }

        private List<Transform> GetNearbyAgents(FlockAgent agent)
        {
            List<Transform> context = new List<Transform>();

            Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, _neighborRadius);

            foreach (var collider in contextColliders)
            {
                if (collider != agent.AgentCollider)
                {
                    context.Add(collider.transform);
                }
            }

            return context;
        }
    }
}

