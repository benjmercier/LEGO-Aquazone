using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LEGOAquazone.Scripts.Test
{
    public class Agent : MonoBehaviour
    {
        

        public void MoveAgent(Vector3 velocity)
        {
            transform.position += velocity * Time.deltaTime;
            transform.forward += velocity * Time.deltaTime;
        }
    }
}

