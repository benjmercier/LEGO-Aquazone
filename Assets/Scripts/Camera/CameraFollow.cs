using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LEGOAquazone.Scripts.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private GameObject _target;

        private Vector3 _targetPos;
        private Vector3 _currentPos;

        private Vector3 _offset = new Vector3(0f, 2.5f, -7.5f);
        private float _dist = 0.25f;

        private Vector3 _one = Vector3.one;

        private void Start()
        {
            
        }

        private void LateUpdate()
        {
            FollowTarget();
        }

        private void FollowTarget()
        {
            _targetPos = _target.transform.position + (_target.transform.rotation * _offset);
                        
            _currentPos = Vector3.SmoothDamp(transform.position, _targetPos, ref _one, _dist);
            transform.position = _currentPos;

            transform.LookAt(_target.transform, _target.transform.up);
        }
    }
}

