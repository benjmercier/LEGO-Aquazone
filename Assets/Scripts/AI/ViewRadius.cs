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

        public static event Action<FlockAgent, FlockAgent, bool> onViewRadiusTriggered;

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
        }

        private void OnEnable()
        {
            FlockAgent.onSetViewRadius += SetViewRadius;
        }

        private void OnDisable()
        {
            FlockAgent.onSetViewRadius -= SetViewRadius;
        }

        private void SetViewRadius(GameObject parent, float agentFOV)
        {
            if (this.transform.parent.gameObject == parent)
            {
                transform.localScale = Vector3.one;
                transform.localScale *= agentFOV;
            }
        }

        private void OnViewRadiusTriggered(FlockAgent neighborAgent, bool isEntering)
        {
            onViewRadiusTriggered?.Invoke(_flockAgent, neighborAgent, isEntering);
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckForFlockAgent(other, true);
        }

        private void OnTriggerExit(Collider other)
        {
            CheckForFlockAgent(other, false);
        }

        private void CheckForFlockAgent(Collider collider, bool isEntering)
        {
            if (collider.TryGetComponent(out IAgent<FlockAgent> flockAgent))
            {
                // invoke event adding agent to list of agents in FOV
                OnViewRadiusTriggered(flockAgent.Agent, isEntering);
            }
        }
    }
}

