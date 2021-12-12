using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _agentsInFlock = new List<GameObject>();

    [SerializeField]
    private int _numAgentsPerType = 20;
    private GameObject _tempAgent;
    private Vector3 _spawnPos;

    [SerializeField]
    private List<Agent> _activeAgents = new List<Agent>();

    private void Start()
    {
        foreach (var agent in _agentsInFlock)
        {
            for (int i = 0; i < _numAgentsPerType; i++)
            {
                SpawnAgent(agent);
            }
        }
    }

    private void Update()
    {
        foreach (var agent in _activeAgents)
        {
            agent.SimulateMovement(_activeAgents, Time.deltaTime);
        }
    }

    private void SpawnAgent(GameObject agentToSpawn)
    {
        _tempAgent = Instantiate(agentToSpawn);

        _spawnPos = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        _tempAgent.transform.localPosition += _spawnPos;
        _tempAgent.transform.localRotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

        if (_tempAgent.TryGetComponent(out Agent agent))
        {
            _activeAgents.Add(agent);
        }
    }
}
