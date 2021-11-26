using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LEGOAquazone.Scripts.AI.Flocking;

namespace LEGOAquazone.Scripts.ScriptableObjects.Behaviors
{
    [CreateAssetMenu(fileName = "Composite.asset", menuName = "Behavior/Composite")]
    public class Composite : FlockBehavior
    {
        public Behavior[] behaviors;

        public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
        {
            Vector3 move = Vector3.zero;

            // iterate through behaviors
            for (int i = 0; i < behaviors.Length; i++)
            {
                Vector3 partialMove = behaviors[i].behavior.CalculateMove(agent, context, flock) * behaviors[i].weight;

                if (partialMove != Vector3.zero)
                {
                    if (partialMove.sqrMagnitude > behaviors[i].weight * behaviors[i].weight)
                    {
                        partialMove.Normalize();
                        partialMove *= behaviors[i].weight;
                    }

                    move += partialMove;
                }
            }

            return move;
        }
    }

    [System.Serializable]
    public class Behavior
    {
        public string name;
        public FlockBehavior behavior;
        public float weight;

        public Behavior(string name, FlockBehavior behavior, float weight)
        {
            this.name = name;
            this.behavior = behavior;
            this.weight = weight;
        }
    }
}

