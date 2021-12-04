using System;
using System.Collections;
using System.Collections.Generic;
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
        public AgentEyeSight[] AgentEyes { get { return _agentEyes; } }

        public static Action<FlockAgent, FlockAgent, bool> onViewRadiusTriggered;

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

            FlockAgent.onUpdateAgentsInFOV += CheckFlockAgentInFOV;
        }

        private void OnDisable()
        {
            FlockAgent.onUpdateAgentsInFOV -= CheckFlockAgentInFOV;
        }

        private void SetViewRadius(float agentFOV)
        {
            transform.localScale = Vector3.one;
            transform.localScale *= agentFOV;
        }                

        private void OnTriggerEnter(Collider other)
        {
            CheckFlockAgentInRadius(other, true);
        }

        private void OnTriggerExit(Collider other)
        {
            CheckFlockAgentInRadius(other, false);
        }

        private void CheckFlockAgentInRadius(Collider collider, bool isEntering)
        {
            if (collider.TryGetComponent(out IAgent<FlockAgent> flockAgent))
            {
                // invoke event adding agent to list of agents in FOV
                OnViewRadiusTriggered(flockAgent.Agent, isEntering);
            }
        }

        private void OnViewRadiusTriggered(FlockAgent neighborAgent, bool isEntering)
        {
            onViewRadiusTriggered?.Invoke(_flockAgent, neighborAgent, isEntering);
        }

        private bool? CheckFlockAgentInFOV(FlockAgent parent, FlockAgent neighbor)
        {
            if (_flockAgent == parent)
            {
                foreach (var eye in _agentEyes)
                {
                    if (eye.CheckIfInEyeFOV(neighbor.gameObject.transform.position))
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                return null;
            }
        }
    }
}

