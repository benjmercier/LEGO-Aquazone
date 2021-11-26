using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LEGOAquazone.Scripts.AI.Flocking;

namespace LEGOAquazone.Scripts.ScriptableObjects.Behaviors
{
    [CreateAssetMenu(fileName = "Alignment.asset", menuName = "Behavior/Alignment")]
    public class Alignment : FlockBehavior
    {
        public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
        {
            // if no neighbors, maintain current alignment
            if (context.Count == 0)
            {
                return agent.transform.forward;
            }

            // add all points together and average
            Vector3 alignmentMove = Vector3.zero;

            foreach (var item in context)
            {
                alignmentMove += item.transform.forward;
            }

            alignmentMove /= context.Count;

            return alignmentMove;
        }
    }
}

