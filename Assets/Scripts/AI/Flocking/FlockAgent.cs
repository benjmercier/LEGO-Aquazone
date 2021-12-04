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
        
        [SerializeField]
        private List<FlockAgent> _neighborAgents = new List<FlockAgent>();

        private void OnEnable()
        {
            ViewRadius.onSetAgentsInFOV += SetAgentsInFOV;
        }

        private void OnDisable()
        {
            ViewRadius.onSetAgentsInFOV -= SetAgentsInFOV;
        }

        private void Update()
        {
            
        }

        private void MoveAgent()
        {
            _cohesionVector = CalculateCohesion();
        }

        private Vector3 CalculateCohesion()
        {
            return Vector3.one;
        }

        public void SetAgentsInFOV(FlockAgent parent, List<FlockAgent> agentsInFOV)
        {
            if (parent == this)
            {
                _neighborAgents = agentsInFOV;
            }
        }
    }
}

