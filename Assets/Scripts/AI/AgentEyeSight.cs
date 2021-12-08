using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LEGOAquazone.Scripts.Interfaces;

namespace LEGOAquazone.Scripts.AI
{
    public class AgentEyeSight : MonoBehaviour, ISeeable
    {
        private enum Eye
        {
            Right = 1,
            Left = -1
        }

        [SerializeField]
        private Eye _eyeSide = Eye.Right;

        [SerializeField]
        private Vector3 _eyeRotation = new Vector3(0f, 45f, 0f);
        private Vector3 _rotationAngle;

        private Vector3 _targetVector;
        private float _dotAngle;
        [SerializeField, Range(-1, 1), Tooltip("1 = directly in front, -1 = directly behind")]
        private float _maxDotProduct = -0.75f;

        private void Start()
        {
            SetEyeRotation();
        }

        private void SetEyeRotation()
        {
            _rotationAngle = _eyeRotation;
            _rotationAngle.y *= (float)_eyeSide;

            transform.localRotation = Quaternion.Euler(_rotationAngle.x, _rotationAngle.y, _rotationAngle.z);
        }

        public bool CheckIfInEyeFOV(Vector3 objPos)
        {
            _targetVector = objPos - transform.position;

            _dotAngle = Vector3.Dot(_targetVector.normalized, transform.forward);
            
            return _dotAngle > _maxDotProduct;
        }
    }
}

