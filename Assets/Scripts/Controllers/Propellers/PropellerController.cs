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
        private float _rotationMultiplier = 35f;

        private float _moveDirection;
        private float _turnDirection;

        private float _rightTurn;
        private float _leftTurn;
        private float _defaultTurn = 0f;

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

        [SerializeField]
        private Propeller[] _activePropellers;

        private void OnEnable()
        {
            _activePropellers = GetComponentsInChildren<Propeller>();

            Steering.onGetPlayerCurrentVelocity += SetPropellerVelocity;
        }

        private void OnDisable()
        {
            Steering.onGetPlayerCurrentVelocity -= SetPropellerVelocity;
        }

        private void SetPropellerVelocity(Transform rootObj, Vector3 currentVelocity)
        {
            _turnDirection = currentVelocity.x;
            _moveDirection = currentVelocity.z;
        }  

        public Quaternion CalculateRotation(PropellerType propellerType)
        {
            var movement = _moveDirection;
            var turn = _turnDirection;

            movement *= _rotationMultiplier;

            if (propellerType == PropellerType.MiddleRear)
            {
                return Quaternion.AngleAxis(movement * Time.deltaTime, Vector3.forward);
            }
            else
            {
                _rightTurn = movement;
                _leftTurn = movement;

                SetMovementDirection(movement);
                SetTurnDirection(turn);

                CalculatePropellerDirection(turn);

                switch (propellerType)
                {
                    case PropellerType.RightRear:

                        return Quaternion.AngleAxis(_rightTurn * Time.deltaTime, Vector3.forward);

                    case PropellerType.LeftRear:

                        return Quaternion.AngleAxis(_leftTurn * Time.deltaTime, Vector3.forward);

                    default:
                        
                        return Quaternion.AngleAxis(_defaultTurn * Time.deltaTime, Vector3.forward);
                }
            }
        }

        private void SetMovementDirection(float moveDirection)
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

        private void CalculatePropellerDirection(float turn)
        {
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
                    CalculatePropellerRotation(turn, ref _rightTurn, ref _leftTurn, -1f);
                }
                else if (_currentTurnDirection == TurnDirection.Left)
                {
                    CalculatePropellerRotation(turn, ref _leftTurn, ref _rightTurn, -1f);
                }
            }
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
    }
}

