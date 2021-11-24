using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LEGOAquazone.Scripts.AI.Steering.Behaviors
{
    [System.Serializable]
    public class Seek
    {
        private Vector3 _seekForce;
                
        public bool calculateArrival;
        
        [SerializeField]
        private float _approachRadius = 2.5f;
        private float _approachDistance;

        public Seek(Vector3 force, bool arrival, float radius, float distance)
        {
            this._seekForce = force;
            this.calculateArrival = arrival;
            this._approachRadius = radius;
            this._approachDistance = distance;
        }

        public Vector3 CalculateSeek()
        {
            return _seekForce;
        }
    }
}

