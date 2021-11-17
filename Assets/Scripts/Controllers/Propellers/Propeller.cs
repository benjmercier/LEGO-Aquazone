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

        private PropellerController _propellerController;

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

            if (transform.parent.root.TryGetComponent(out PropellerController propellerController))
            {
                _propellerController = propellerController;
            }

            if (_isPlayer)
            {
                
            }
        }

        private void OnDisable()
        {
            if (_isPlayer)
            {
               
            }
        }

        /// <summary>
        /// need to check if root game obj has propeller controller
        /// if so, propeller added to list of active/available propellers
        /// </summary>

        private void Update()
        {
            if (_propellerController != null)
            {
                //RotatePropeller();
            }            
        }

        private void RotatePropeller()
        {
            gameObject.transform.rotation *= _propellerController.CalculateRotation(propellerType);
        }
    }
}
