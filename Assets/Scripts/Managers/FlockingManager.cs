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
        private Transform _flockOrigin;

        [SerializeField]
        private Vector3 _flockArea = new Vector3(5f, 5f, 5f);

        private void Start()
        {
            for (int i = 0; i < _availableAgents; i++)
            {
                Vector3 spawnPos = _flockOrigin.position + new Vector3(
                    Random.Range(-_flockArea.x, _flockArea.x),
                    Random.Range(-_flockArea.y, _flockArea.y),
                    Random.Range(-_flockArea.z, _flockArea.z));

                var agent = Instantiate(_agentPrefab, spawnPos, Quaternion.identity);

                _activeAgents.Add(agent);
            }
        }
    }
}

