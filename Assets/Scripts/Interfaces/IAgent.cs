using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LEGOAquazone.Scripts.AI.Flocking;

namespace LEGOAquazone.Scripts.Interfaces
{
    public interface IAgent<T>
    {
        T Agent { get; }
    }
}

