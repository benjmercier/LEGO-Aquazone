using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LEGOAquazone.Scripts.AI.Flocking;

namespace LEGOAquazone.Scripts.ScriptableObjects.Behaviors
{
    [CreateAssetMenu(fileName = "Avoidance.asset", menuName = "Behavior/Avoidance")]
    public class Avoidance : FlockBehavior
    {
        public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
        {
            // if no neighbors, return no adjustment
            if (context.Count == 0)
            {
                return Vector3.zero;
            }

            // add all points together and average
            Vector3 avoidanceMove = Vector3.zero;
            int nAvoid = 0;

            foreach (var item in context)
            {
                if (Vector3.SqrMagnitude(item.position - agent.transform.position) < flock.SqrAvoidanceRadius)
                {
                    nAvoid++;
                    avoidanceMove += agent.transform.position - item.position;
                }
                
            }

            if (nAvoid > 0)
            {
                avoidanceMove /= nAvoid;
            }

            return avoidanceMove;
        }
    }
}

