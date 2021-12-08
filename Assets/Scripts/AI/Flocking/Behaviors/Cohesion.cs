using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LEGOAquazone.Scripts.AI.Flocking.Behaviors
{
    public class Cohesion
    {
        private Vector3 _cohesionVector;

        public Vector3 CalculateCohesion(Transform agentTransform, List<FlockAgent> neighborAgents)
        {
            _cohesionVector = Vector3.zero;

            if (neighborAgents.Count > 0)
            {
                neighborAgents.ForEach(agent => _cohesionVector += agent.transform.position);

                _cohesionVector /= neighborAgents.Count;
                _cohesionVector -= agentTransform.position;
                _cohesionVector.Normalize();
            }

            return _cohesionVector;
        }
    }
}

