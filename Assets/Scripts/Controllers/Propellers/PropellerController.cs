using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LEGOAquazone.Scripts.Player.Movement;

namespace LEGOAquazone.Scripts.Controllers.Propellers
{
    [RequireComponent(typeof(Steering))]
    public class PropellerController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _leftPropeller,
            _midPropeller,
            _rightPropeller;

        private float _rotationMultiplier = 35f;

        private float _rightTurn;
        private float _leftTurn;        
                
        private float _moveDirection;
        private float _turnDirection;

        private enum MoveDirection
        {
            Forward,
            Backward,
            InPlace
        }

        public Propeller[] propellers;

        private void OnEnable()
        {
            propellers = GetComponentsInChildren<Propeller>();

            Steering.onGetPlayerCurrentVelocity += SetDirection;
        }

        private void OnDisable()
        {
            Steering.onGetPlayerCurrentVelocity -= SetDirection;
        }

        // Update is called once per frame
        void Update()
        {
           CalculatePropellerDirection(_moveDirection, _turnDirection);
        }

        private void SetDirection(Transform rootObj, Vector3 currentVelocity)
        {
            _turnDirection = currentVelocity.x;
            _moveDirection = currentVelocity.z;
        }               

        private void CalculatePropellerDirection(float moveDirection, float turnDirection)
        {
            // if moveDirection > 0 = forward
            // positive rotation
            // else if < 0 = backward
            // negative rotation
            
            moveDirection *= _rotationMultiplier;
            _midPropeller.transform.rotation *= Quaternion.AngleAxis(moveDirection * Time.deltaTime, Vector3.forward);

            _leftTurn = moveDirection;
            _rightTurn = moveDirection;


            //
            CalculatePropellerRotation(moveDirection, turnDirection, ref _leftTurn);
            CalculatePropellerRotation(moveDirection, turnDirection, ref _rightTurn);
            //





            if (moveDirection > 0) // moving forward
            {
                if (turnDirection > 0) // turn right
                {
                    CalculatePropellerRotation(ref _rightTurn, ref _leftTurn);
                }
                else if (turnDirection < 0) // turn left
                {
                    CalculatePropellerRotation(ref _leftTurn, ref _rightTurn);
                }                
            }
            else if (moveDirection < 0) // moving backwards
            {
                if (turnDirection > 0) // turn right
                {
                    CalculatePropellerRotation(ref _rightTurn, ref _leftTurn, -1f, -1f);
                }
                else if (turnDirection < 0) // turn left
                {
                    CalculatePropellerRotation(ref _leftTurn, ref _rightTurn, -1f, -1f);
                }
            }
            else // in place
            {
                if (turnDirection > 0) // turn right
                {
                    CalculatePropellerRotation(turnDirection, ref _rightTurn, ref _leftTurn, -1f);
                }
                else if (turnDirection < 0) // turn left
                {
                    CalculatePropellerRotation(turnDirection, ref _leftTurn, ref _rightTurn, -1f);
                }
            }

            _rightPropeller.transform.rotation *= Quaternion.AngleAxis(_rightTurn * Time.deltaTime, Vector3.forward);
            _leftPropeller.transform.rotation *= Quaternion.AngleAxis(_leftTurn * Time.deltaTime, Vector3.forward);            
        }

        private void CalculatePropellerRotation(float moveDirection, float turnDirection, ref float turn)
        {
            if (moveDirection < 0)
            {
                
            }

            if (turnDirection < 0)
            {
                
            }
            
        }

        private void CalculatePropellerRotation(ref float turnTo, ref float turnFrom, float toDirection = 1f, float fromDirection = 1f)
        {
            turnTo *= toDirection * 0.5f;
            turnFrom *= fromDirection;
        }

        private void CalculatePropellerRotation(float turnDirection, ref float turnTo, ref float turnFrom,  float toDirection = 1f, float fromDirection = 1f)
        {
            turnTo = Mathf.Abs(turnDirection * _rotationMultiplier) * toDirection;
            turnTo *= 0.5f;

            turnFrom = Mathf.Abs(turnDirection * _rotationMultiplier) * fromDirection;
        }

        public Quaternion CalculateRotation(Propeller.PropellerType type)
        {
            var move = _moveDirection * _rotationMultiplier;

            _rightTurn = move;
            _leftTurn = move;            

            if (type == Propeller.PropellerType.Middle)
            {
                return Quaternion.AngleAxis(move * Time.deltaTime, Vector3.forward);
            }
            else
            {

            }


            _moveDirection *= _rotationMultiplier;

            float turn = _moveDirection;

            return Quaternion.AngleAxis(turn * Time.deltaTime, Vector3.forward);
        }
    }
}

