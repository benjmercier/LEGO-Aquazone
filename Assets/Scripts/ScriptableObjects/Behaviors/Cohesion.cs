using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LEGOAquazone.Scripts.AI.Flocking;

namespace LEGOAquazone.Scripts.ScriptableObjects.Behaviors
{
    [CreateAssetMenu(fileName = "Cohesion.asset", menuName = "Behavior/Cohesion")]
    public class Cohesion : FlockBehavior
    {
        public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
        {
            // if no neighbors, return no adjustment
            if (context.Count == 0)
            {
                return Vector3.zero;
            }

            // add all points together and average
            Vector3 cohesionMove = Vector3.zero;

            foreach (var item in context)
            {
                cohesionMove += item.position;
            }

            cohesionMove /= context.Count;

            // create offset from agent pos
            cohesionMove -= agent.transform.position;

            return cohesionMove;
        }
    }
}

