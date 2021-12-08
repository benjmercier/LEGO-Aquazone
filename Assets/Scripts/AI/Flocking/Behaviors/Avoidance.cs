using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LEGOAquazone.Scripts.AI.Flocking.Behaviors
{
    public class Avoidance : MonoBehaviour
    {
        private Vector3 _avoidanceVector;

        public Vector3 CalculateAvoidance(Transform agentTransform, List<FlockAgent> neighborAgents)
        {
            _avoidanceVector = Vector3.zero;

            if (neighborAgents.Count > 0)
            {
                neighborAgents.ForEach(agent => _avoidanceVector += (agentTransform.position - agent.transform.position));

                _avoidanceVector /= neighborAgents.Count;
                _avoidanceVector.Normalize();
            }

            return _avoidanceVector;
        }
    }
}

