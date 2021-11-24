using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LEGOAquazone.Scripts.Managers
{
    public class FlockingManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _agentPrefab;

        [SerializeField]
        private int _availableAgents = 20;

        [SerializeField]
        private List<GameObject> _activeAgents = new List<GameObject>();

        [SerializeField]
        private Transform _spawnOrigin;

        [SerializeField]
        private Vector3 _spawnArea = new Vector3(5f, 5f, 5f);

        [Header("Flock Settings")]
        [SerializeField, Range(0.0f, 5.0f)]
        private float _minSpeed;
        [SerializeField, Range(0.0f, 5.0f)]
        private float _maxSpeed;
        [SerializeField, Range(0.5f, 10.0f)]
        private float _neighborDistance;
        [SerializeField, Range(0.0f, 5.0f)]
        private float _rotationSpeed;

        private void Start()
        {
            for (int i = 0; i < _availableAgents; i++)
            {
                Vector3 spawnPos = _spawnOrigin.position + new Vector3(
                    Random.Range(-_spawnArea.x, _spawnArea.x),
                    Random.Range(-_spawnArea.y, _spawnArea.y),
                    Random.Range(-_spawnArea.z, _spawnArea.z));

                var agent = Instantiate(_agentPrefab, spawnPos, Quaternion.identity);

                _activeAgents.Add(agent);
            }
        }
    }
}

