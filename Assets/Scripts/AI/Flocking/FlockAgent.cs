using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LEGOAquazone.Scripts.AI.Flocking
{
    [RequireComponent(typeof(Collider))]
    public class FlockAgent : MonoBehaviour
    {
        private Collider _agentCollider;
        public Collider AgentCollider { get { return _agentCollider; } }

        private void Start()
        {
            if (TryGetComponent(out Collider collider))
            {
                _agentCollider = collider;
            }
            else
            {
                Debug.Log("No collider on " + this.name);
            }
        }

        public void MoveAgent(Vector3 velocity)
        {
            transform.forward = velocity;
            transform.position += velocity * Time.deltaTime;
        }
    }
}

