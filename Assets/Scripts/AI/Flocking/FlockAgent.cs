using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        //[SerializeField]
        //private float _agentFOV = 5f;
        [SerializeField]
        private List<FlockAgent> _agentsInViewRadius = new List<FlockAgent>();
        [SerializeField]
        private List<FlockAgent> _agentsInFOV = new List<FlockAgent>();

        //private Vector3 _targetVector;
        //private float _dotAngle;
        //[SerializeField, Tooltip("1 = directly in front, -1 = directly behind")]
        //private float _angleOffset = -0.75f;


        public static Func<FlockAgent, FlockAgent, bool?> onUpdateAgentsInFOV;


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
            
        }

        private void Update()
        {
            //CalculateAgentsInFOV();

            UpdateAgentsInFOV();
        }

        private void MoveAgent()
        {
            _cohesionVector = CalculateCohesion();
        }

        private void UpdateAgentsInRange(FlockAgent parentAgent, FlockAgent neighborAgent, bool inRange)
        {
            if (parentAgent == this)
            {
                if (inRange)
                {                    
                    _agentsInViewRadius.Add(neighborAgent);
                }
                else
                {
                    _agentsInViewRadius.Remove(neighborAgent);
                    if (_agentsInFOV.Contains(neighborAgent))
                    {
                        _agentsInFOV.Remove(neighborAgent);
                    }
                }
            }
        }

        private void UpdateAgentsInFOV()
        {
            if (_agentsInViewRadius.Count > 0)
            {
                AgentEyeSight[] eyes = gameObject.GetComponentsInChildren<AgentEyeSight>();

                foreach (var agent in _agentsInViewRadius)
                {
                    if (eyes.Any(e => e.CheckIfInEyeFOV(agent.gameObject.transform.position)))
                    {
                        if (!_agentsInFOV.Contains(agent))
                        {
                            _agentsInFOV.Add(agent);
                        }
                    }
                    else
                    {
                        if (_agentsInFOV.Contains(agent))
                        {
                            _agentsInFOV.Remove(agent);
                        }
                    }


                    /*
                    if (OnUpdateAgentsInFOV(agent) == true)
                    {
                        if (!_agentsInFOV.Contains(agent))
                        {
                            _agentsInFOV.Add(agent);
                        }
                    }
                    else if (OnUpdateAgentsInFOV(agent) == false)
                    {
                        if (_agentsInFOV.Contains(agent))
                        {
                            _agentsInFOV.Remove(agent);
                        }
                    }*/
                }
            }
        }

        // check agents in range via radius
        // if in range, add to _agentsInViewRadius
        // if _agentsInViewRadius > 0
        //   check each agent to see if in FOV
        //   if in FOV
        //      add to _agentsInFOV

        // talk to ViewRadius
        // get back agents in FOV via AgentEyeSight
        private bool? OnUpdateAgentsInFOV(FlockAgent agent)
        {
            return onUpdateAgentsInFOV?.Invoke(this, agent);
        }


       

        /*
        private void CalculateAgentsInFOV()
        {
            if (_agentsInViewRadius.Count > 0)
            {
                foreach (var agent in _agentsInViewRadius)
                {
                    if (CheckNeighborPos(agent.transform.position))
                    {
                        agent._renderer.material.color = Color.red;

                        if (!_agentsInFOV.Contains(agent))
                            _agentsInFOV.Add(agent);
                    }
                    else
                    {
                        agent._renderer.material.color = agent._color;
                        if (_agentsInFOV.Contains(agent))
                            _agentsInFOV.Remove(agent);
                    }
                }
            }
        }*/

        private Vector3 CalculateCohesion()
        {
            return Vector3.one;
        }

        
    }
}

