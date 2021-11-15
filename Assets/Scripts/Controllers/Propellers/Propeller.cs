using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LEGOAquazone.Scripts.Player.Movement;

namespace LEGOAquazone.Scripts.Controllers.Propellers
{
    public class Propeller : MonoBehaviour
    {
        [SerializeField]
        private bool _isPlayer = false;

        public enum PropellerType
        {
            Right,
            Left,
            Middle
        }

        public PropellerType propellerType;

        private float _moveDirection;
        private float _turnDirection;

        private Func<Transform, Vector3> returnCurrentVelocity;

        private void OnEnable()
        {
            if (transform.parent.root.CompareTag("Player"))
            {
                _isPlayer = true;
            }
            else
            {
                _isPlayer = false;
            }

            if (_isPlayer)
            {
                Steering.onGetPlayerCurrentVelocity += GetCurrentVelocity;
            }
        }

        private void OnDisable()
        {
            if (_isPlayer)
            {
                Steering.onGetPlayerCurrentVelocity -= GetCurrentVelocity;
            }
        }

        private void GetCurrentVelocity(Transform rootObj, Vector3 currentVelocity)
        {
            if (transform.parent.root == rootObj)
            {
                Debug.Log("Found the parent.");
            }
        }
    }
}
