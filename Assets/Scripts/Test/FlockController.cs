using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LEGOAquazone.Scripts.Test
{
    public class FlockController : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _agentPrefabs = new List<GameObject>();
        private List<GameObject> _activeAgents = new List<GameObject>();
    }
}

