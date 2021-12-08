using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LEGOAquazone.Scripts.AI.Flocking.Behaviors
{
    public class Alignment
    {
        private Vector3 _alignmentVector;

        public Vector3 CalculateAlignment(Transform agentTransform, List<FlockAgent> neighborAgents)
        {
            _alignmentVector = agentTransform.forward;

            if (neighborAgents.Count > 0)
            {
                neighborAgents.ForEach(agent => _alignmentVector += agent.transform.forward);

                _alignmentVector /= neighborAgents.Count;
                _alignmentVector.Normalize();
            }

            return _alignmentVector;
        }
    }
}

