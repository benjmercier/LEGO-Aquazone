using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LEGOAquazone.Scripts.AI.Flocking;

namespace LEGOAquazone.Scripts.ScriptableObjects.Behaviors
{
    [CreateAssetMenu(fileName = "Containment.asset", menuName = "Behavior/Containment")]
    public class Containment : FlockBehavior
    {
        [SerializeField]
        private Vector3 _origin;
        [SerializeField]
        private float _radius;

        public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
        {
            Vector3 originOffset = _origin - agent.transform.position;
            float t = originOffset.magnitude / _radius;

            if (t < 0.9f)
            {
                return Vector3.zero;
            }

            return originOffset * t * t;
        }
    }
}

