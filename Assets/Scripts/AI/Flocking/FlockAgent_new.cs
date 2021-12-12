using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LEGOAquazone.Scripts.AI.Flocking
{
    public class FlockAgent_new : MonoBehaviour
    {
        [SerializeField]
        private float _maxSpeed = 10f;
        [SerializeField, Range(0f, 1f)]
        private float _maxForce = 0.25f;
        [SerializeField]
        private int _groupIndex;
        public int GroupIndex { get { return _groupIndex; } set { _groupIndex = value; } }

        [SerializeField]
        private float _minObjDistance = 1.5f,
            _maxObjDistance = 5f;

        private Vector3 _cohesionVector,
            _alignmentVector,
            _avoidanceVector,
            _containmentVector,
            _obstacleVector;

        private Vector3 _desiredVelocity,
            _currentVelocity;

        private void Start()
        {
            // use AgentEyeSight to determine neighbor vectors
        }
    }
}
