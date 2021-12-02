using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineMeshes : MonoBehaviour
{
    private MeshFilter _combinedMesh;

    private MeshFilter[] _childMeshFilters;
    private CombineInstance[] _combine;

    private MeshCollider _meshCollider;

    // Start is called before the first frame update
    void Start()
    {
        _childMeshFilters = GetComponentsInChildren<MeshFilter>();
        _combine = new CombineInstance[_childMeshFilters.Length];

        int i = 0;

        while (i < _childMeshFilters.Length)
        {
            _combine[i].mesh = _childMeshFilters[i].sharedMesh;
            _combine[i].transform = _childMeshFilters[i].transform.localToWorldMatrix;
            //_childMeshFilters[i].gameObject.SetActive(false);

            i++;
        }

        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(_combine);
        transform.gameObject.SetActive(true);

        _meshCollider = transform.GetComponent<MeshCollider>();
        _meshCollider.sharedMesh = transform.GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
