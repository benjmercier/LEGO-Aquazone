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

        [SerializeField]
        private PropellerController.PropellerType _currentPropellerType;

        private PropellerController _propellerController;

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
        }

        // each propeller talks to propellerController at root of game object
        // passes in what type of propeller (right, left, mid)
        // gets back quaternion to rotate by

        private void Update()
        {
            if (_propellerController != null)
            {
                RotatePropeller();
            }            
        }

        private void RotatePropeller()
        {
            gameObject.transform.rotation *= _propellerController.CalculateRotation(_currentPropellerType);
        }
    }
}
