using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LEGOAquazone.Scripts.AI.Flocking;
using LEGOAquazone.Scripts.Interfaces;

namespace LEGOAquazone.Scripts.AI
{
    [RequireComponent(typeof(SphereCollider))]
    public class ViewRadius : MonoBehaviour
    {
        [SerializeField]
        private FlockAgent _flockAgent;

        [SerializeField]
        private float _agentFOV = 5f;

        [SerializeField]
        private AgentEyeSight[] _agentEyes;

        [SerializeField]
        private List<FlockAgent> _agentsInViewRadius = new List<FlockAgent>();
        [SerializeField]
        private List<FlockAgent> _agentsInFOV = new List<FlockAgent>();

        public static event Action<FlockAgent, List<FlockAgent>> onSetAgentsInFOV;

        private void Awake()
        {
            if (_flockAgent == null)
            {
                if (transform.parent.TryGetComponent(out FlockAgent flockAgent))
                {
                    _flockAgent = flockAgent;
                }
                else
                {
                    Debug.Log("ViewRadius::Awake()::FlockAgent is NULL on " + transform.root.name);
                }
            }

            _agentEyes = GetComponentsInChildren<AgentEyeSight>();
        }

        private void OnEnable()
        {
            SetViewRadius(_agentFOV);
        }

        private void Update()
        {
            CheckFlockAgentsInFOV();
        }

        private void SetViewRadius(float agentFOV)
        {
            transform.localScale = Vector3.one;
            transform.localScale *= agentFOV;
        }                

        private void OnTriggerEnter(Collider other)
        {
            CheckForFlockAgentInRadius(other, true);
        }

        private void OnTriggerExit(Collider other)
        {
            CheckForFlockAgentInRadius(other, false);
        }

        private void CheckForFlockAgentInRadius(Collider collider, bool isEntering)
        {
            if (collider.TryGetComponent(out IAgent<FlockAgent> flockAgent))
            {
                UpdateAgentsInRadius(flockAgent.Agent, isEntering);
            }
        }

        private void UpdateAgentsInRadius(FlockAgent flockAgent, bool inRadius)
        {
            if (inRadius)
            {
                _agentsInViewRadius.Add(flockAgent);
            }
            else
            {
                _agentsInViewRadius.Remove(flockAgent);

                ModifyAgentsInFOV(flockAgent, false);
            }
        }

        private void CheckFlockAgentsInFOV()
        {
            if (_agentsInViewRadius.Any())
            {
                _agentsInViewRadius.ForEach(agent =>
                {
                    if (_agentEyes.Any(eye => eye.CheckIfInEyeFOV(agent.transform.position)))
                    {
                        ModifyAgentsInFOV(agent, true);
                    }
                    else
                    {
                        ModifyAgentsInFOV(agent, false);
                    }
                });
            }

            //if (_agentsInViewRadius.Count > 0)
            //{
            //    foreach (var agent in _agentsInViewRadius)
            //    {
            //        if (_agentEyes.Any(eye => eye.CheckIfInEyeFOV(agent.transform.position)))
            //        {
            //            ModifyAgentsInFOV(agent, true);
            //        }
            //        else
            //        {
            //            ModifyAgentsInFOV(agent, false);
            //        }
            //    }
            //}
        }

        private void ModifyAgentsInFOV(FlockAgent agent, bool addToList)
        {
            if (addToList)
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

            OnSetAgentsInFOV(_flockAgent, _agentsInFOV);
        }

        private void OnSetAgentsInFOV(FlockAgent parent, List<FlockAgent> agentsInFOV)
        {
            onSetAgentsInFOV?.Invoke(parent, agentsInFOV);
        }
    }
}

