using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LEGOAquazone.Scripts.Interfaces;
using LEGOAquazone.Scripts.AI;

namespace LEGOAquazone.Scripts.AI.Flocking
{
    [SelectionBase]
    public class FlockAgent : MonoBehaviour, IAgent<FlockAgent>
    {
        public FlockAgent Agent { get { return this; } }

        private Vector3 _cohesionVector;
        [SerializeField]
        private float _agentFOV = 5f;
        [SerializeField]
        private List<FlockAgent> _agentsInFOV = new List<FlockAgent>();

        public static event Action<GameObject, float> onSetViewRadius;

        [Header("Test")]
        public Renderer _renderer;
        public Color _color;

        private void Awake()
        {
            _renderer = gameObject.GetComponentInChildren<Renderer>();
            _color = _renderer.material.color;
        }

        private void OnEnable()
        {
            ViewRadius.onViewRadiusTriggered += UpdateAgentsInRange;
        }

        private void OnDisable()
        {
            ViewRadius.onViewRadiusTriggered -= UpdateAgentsInRange;
        }

        private void Start()
        {
            OnSetViewRadius(_agentFOV);
        }

        private void Update()
        {
            
        }

        private void OnSetViewRadius(float agentFOV)
        {
            /*
            var view = transform.GetComponentInChildren<ViewRadius>();
            if (view != null)
            {
                view.gameObject.transform.localScale = new Vector3(agentFOV, agentFOV, agentFOV);
            }*/

            onSetViewRadius?.Invoke(this.gameObject, agentFOV);
        }

        private void MoveAgent()
        {
            CalculateAgentsInFOV();

            _cohesionVector = CalculateCohesion();
        }

        private void UpdateAgentsInRange(FlockAgent parentAgent, FlockAgent neighborAgent, bool inRange)
        {
            if (parentAgent == this)
            {
                if (inRange)
                {                    
                    _agentsInFOV.Add(neighborAgent);
                }
                else
                {
                    _agentsInFOV.Remove(neighborAgent);
                }
            }
        }

        private void CalculateAgentsInFOV()
        {
            if (_agentsInFOV.Count > 0)
            {
                foreach (var agent in _agentsInFOV)
                {
                    if (CheckAgentPos(agent.transform.position))
                    {
                        agent._renderer.material.color = Color.red;
                    }
                    else
                    {
                        agent._renderer.material.color = agent._color;
                    }
                }
            }
        }

        private bool CheckAgentPos(Vector3 agentPos)
        {
            var targetVector = agentPos - transform.position;

            float dotAngle = Vector3.Dot(targetVector.normalized, transform.forward);

            // -1 = directly behind, 1 = directly in front
            float angleOffset = -0.75f;

            return dotAngle > angleOffset;
        }

        private Vector3 CalculateCohesion()
        {
            return Vector3.one;
        }

        
    }
}

