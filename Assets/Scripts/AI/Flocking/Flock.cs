using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LEGOAquazone.Scripts.AI.Flocking
{
    public class Flock : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _flockAgentPrefabs = new List<GameObject>();
        [SerializeField]
        private Transform _flockContainer;
        [SerializeField]
        private int _flockSize = 20;
        [SerializeField]
        private Transform _flockOrigin;
        [SerializeField]
        private float _flockRadius = 10f;

        private Vector3 _randomSpawn;
        private Vector3 _spawnPos;
        private Quaternion _spawnRot;
        private GameObject _tempAgent;

        [SerializeField]
        private List<FlockAgent> _activeFlockAgents = new List<FlockAgent>();

        private void Start()
        {
            GenerateFlockAgents();
        }

        private void GenerateFlockAgents()
        {
            foreach (var agent in _flockAgentPrefabs)
            {
                for (int i = 0; i < _flockSize; i++)
                {
                    _randomSpawn = Random.insideUnitSphere;
                    _randomSpawn *= _flockRadius;

                    _spawnPos = _flockOrigin.position + _randomSpawn;
                    _spawnRot = Quaternion.Euler(0, Random.Range(0, 360), 0);

                    _tempAgent = Instantiate(agent, _spawnPos, _spawnRot);
                    _tempAgent.transform.parent = _flockContainer;

                    if (_tempAgent.TryGetComponent(out FlockAgent flockAgent))
                    {
                        _activeFlockAgents.Add(flockAgent);
                    }
                }
            }
        }
    }
}

