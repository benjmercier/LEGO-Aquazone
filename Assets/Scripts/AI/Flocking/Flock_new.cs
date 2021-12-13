using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LEGOAquazone.Scripts.AI.Flocking
{
    [System.Serializable]
    public class Flock_new
    {
        public GameObject spawnContainer;
        public Transform spawnOrigin;
        public float roamingRadius = 10f;

        [Space]
        public int totalAgents = 50;
        [Range(0, 5)]
        public int totalGroups = 3;
        public List<FlockAgent_new> agentPrefabs = new List<FlockAgent_new>();
        public List<FlockAgent_new> activeAgents = new List<FlockAgent_new>();
    }
}
