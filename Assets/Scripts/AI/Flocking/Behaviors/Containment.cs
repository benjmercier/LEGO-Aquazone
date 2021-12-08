using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LEGOAquazone.Scripts.AI.Flocking.Behaviors
{
    public class Containment
    {
        private Vector3 _containmentVector;

        public Vector3 CalculateContainment(FlockController currentFlock, Transform agentTransform)
        {
            _containmentVector = currentFlock.transform.position - agentTransform.position;

            if (_containmentVector.magnitude >= currentFlock.FlockRadius)
            {
                return _containmentVector.normalized;
            }

            return Vector3.zero;
        }
    }
}

