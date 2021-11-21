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

        public enum PropellerType
        {
            RightRear,
            LeftRear,
            MiddleRear
        }

        private enum MoveDirection
        {
            Forward,
            Backward,
            InPlace
        }

        private MoveDirection _currentMoveDirection;

        private enum TurnDirection
        {
            Right,
            Left,
            Straight
        }

        private TurnDirection _currentTurnDirection;

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

        public void CalculatePropellerDirection(float moveDirection, float turnDirection)
        {
            // if moveDirection > 0 = forward
            // positive rotation
            // else if < 0 = backward
            // negative rotation

            //var moveDirection = _moveDirection;
            //var turnDirection = _turnDirection;
            
            moveDirection *= _rotationMultiplier;
            _midPropeller.transform.rotation *= Quaternion.AngleAxis(moveDirection * Time.deltaTime, Vector3.forward);

            /*
            if (propellerType == PropellerType.MiddleRear)
            {
                return Quaternion.AngleAxis(moveDirection * Time.deltaTime, Vector3.forward);
            }
            */

            _leftTurn = moveDirection;
            _rightTurn = moveDirection;

            SetMoveDirection(moveDirection);
            SetTurnDirection(turnDirection);

            if (_currentMoveDirection == MoveDirection.Forward || _currentMoveDirection == MoveDirection.Backward)
            {
                if (_currentTurnDirection == TurnDirection.Right)
                {
                    CalculatePropellerRotation(ref _rightTurn);
                }
                else if (_currentTurnDirection == TurnDirection.Left)
                {
                    CalculatePropellerRotation(ref _leftTurn);
                }
            }
            else
            {
                if (_currentTurnDirection == TurnDirection.Right)
                {
                    CalculatePropellerRotation(turnDirection, ref _rightTurn, ref _leftTurn, -1f);
                }
                else if (_currentTurnDirection == TurnDirection.Left)
                {
                    CalculatePropellerRotation(turnDirection, ref _leftTurn, ref _rightTurn, -1f);
                }
            }

            /*
            if (propellerType == PropellerType.RightRear)
            {
                return Quaternion.AngleAxis(_rightTurn * Time.deltaTime, Vector3.forward);
            }
            else
            {
                return Quaternion.AngleAxis(_leftTurn * Time.deltaTime, Vector3.forward);
            }
            */
            _rightPropeller.transform.rotation *= Quaternion.AngleAxis(_rightTurn * Time.deltaTime, Vector3.forward);
            _leftPropeller.transform.rotation *= Quaternion.AngleAxis(_leftTurn * Time.deltaTime, Vector3.forward);            


        }

        private void CalculatePropellerRotation(ref float turnTo)
        {
            turnTo *= 0.5f;
        }

        private void CalculatePropellerRotation(float turnDirection, ref float turnTo, ref float turnFrom,  float toDirection = 1f, float fromDirection = 1f)
        {
            turnTo = Mathf.Abs(turnDirection * _rotationMultiplier) * toDirection;
            turnTo *= 0.5f;

            turnFrom = Mathf.Abs(turnDirection * _rotationMultiplier) * fromDirection;
        }

        private void SetMoveDirection(float moveDirection)
        {
            if (moveDirection > 0)
            {
                _currentMoveDirection = MoveDirection.Forward;
            }
            else if (moveDirection < 0)
            {
                _currentMoveDirection = MoveDirection.Backward;
            }
            else
            {
                _currentMoveDirection = MoveDirection.InPlace;
            }
        }
        
        private void SetTurnDirection(float turnDirection)
        {
            if (turnDirection > 0)
            {
                _currentTurnDirection = TurnDirection.Right;
            }
            else if (turnDirection < 0)
            {
                _currentTurnDirection = TurnDirection.Left;
            }
            else
            {
                _currentTurnDirection = TurnDirection.Straight;
            }
        }
    }
}

