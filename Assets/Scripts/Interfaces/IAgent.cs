using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LEGOAquazone.Scripts.Interfaces
{
    public interface IAgent<T>
    {
        T Agent { get; }

        int FlockID { get; set; }
    }
}

