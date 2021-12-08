using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LEGOAquazone.Scripts.AI.Flocking;
using LEGOAquazone.Scripts.Interfaces;

namespace LEGOAquazone.Scripts.AI
{
    [RequireComponent(typeof(SphereCollider))]
    public class ViewRadius : MonoBehaviour
    {
        [SerializeField]
        private float _agentFOV = 5f;

        [SerializeField]
        private AgentEyeSight[] _agentEyes;

        [Header("Agents In Range")]
        [SerializeField]
        private List<GameObject> _agentsInViewRadius = new List<GameObject>();
        [SerializeField]
        private List<GameObject> _agentsInFOV = new List<GameObject>();
        public List<GameObject> AgentsInFOV { get { return _agentsInFOV; } }

        [Header("Obstacles In Range")]
        [SerializeField]
        private List<GameObject> _obstaclesInViewRadius = new List<GameObject>();
        [SerializeField]
        private List<GameObject> _obstaclesInFOV = new List<GameObject>();
        public List<GameObject> ObstaclesInFOV { get { return _obstaclesInFOV; } }

        private void Awake()
        {
            _agentEyes = GetComponentsInChildren<AgentEyeSight>();
        }

        private void OnEnable()
        {
            SetViewRadius(_agentFOV);
        }

        private void Update()
        {
            CheckObjectsInFOV(_agentsInViewRadius, _agentsInFOV);
            CheckObjectsInFOV(_obstaclesInViewRadius, _obstaclesInFOV);
        }

        private void SetViewRadius(float agentFOV)
        {
            transform.localScale = Vector3.one;
            transform.localScale *= agentFOV;
        }                

        private void OnTriggerEnter(Collider other)
        {
            IdentifyObjectsInRadius(other.gameObject, true);
        }

        private void OnTriggerExit(Collider other)
        {
            IdentifyObjectsInRadius(other.gameObject, false);
        }

        private void IdentifyObjectsInRadius(GameObject objInRadius, bool isEntering)
        {
            if (objInRadius.TryGetComponent(out IAgent<FlockAgent> flockAgent))
            {
                UpdateObjectsInRadius(objInRadius.gameObject, _agentsInViewRadius, _agentsInFOV, isEntering);
            }
            else if (objInRadius.TryGetComponent(out IAvoidable avoidable))
            {
                UpdateObjectsInRadius(objInRadius.gameObject, _obstaclesInViewRadius, _obstaclesInFOV, isEntering);
            }
        }

        private void UpdateObjectsInRadius(GameObject objInRadius, List<GameObject> objInRadiusList, List<GameObject> objInFOVList, bool inRadius)
        {
            if (inRadius)
            {
                objInRadiusList.Add(objInRadius);
            }
            else
            {
                objInRadiusList.Remove(objInRadius);

                ModifyObjectsInFOV(objInRadius, objInFOVList, false);
            }
        }

        private void CheckObjectsInFOV(List<GameObject> objInRadiusList, List<GameObject> objInFOVList)
        {
            if (objInRadiusList.Any())
            {
                objInRadiusList.ForEach(obj =>
                {
                    if (_agentEyes.Any(eye => eye.CheckIfInEyeFOV(obj.transform.position)))
                    {
                        ModifyObjectsInFOV(obj, objInFOVList, true);
                    }
                    else
                    {
                        ModifyObjectsInFOV(obj, objInFOVList, false);
                    }
                });
            }
        }

        private void ModifyObjectsInFOV(GameObject objInFOV, List<GameObject> objInFOVList, bool addToList)
        {
            if (addToList)
            {
                if (!objInFOVList.Contains(objInFOV))
                {
                    objInFOVList.Add(objInFOV);
                }
            }
            else
            {
                if (objInFOVList.Contains(objInFOV))
                {
                    objInFOVList.Remove(objInFOV);
                }
            }
        }
    }
}

