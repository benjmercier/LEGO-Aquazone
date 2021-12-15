using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LEGOAquazone.Scripts.AI.Boids
{
    public class Spawner : MonoBehaviour
    {
        public enum GizmoType
        {
            Never,
            SelectedOnly,
            Always
        }

        public Boid[] prefabs;
        public float spawnRadius = 10f;
        public int spawnCount = 10;
        public Color color;
        public GizmoType showSpawnRegion;

        private void Awake()
        {
            foreach (Boid obj in prefabs)
            {
                for (int i = 0; i < spawnCount; i++)
                {
                    Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
                    Boid boid = Instantiate(obj);
                    boid.transform.position = pos;
                    boid.transform.forward = Random.insideUnitSphere;

                    //boid.SetColor(color);
                }
            }
            
        }

        private void OnDrawGizmos()
        {
            if (showSpawnRegion == GizmoType.Always)
            {
                DrawGizmos();
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (showSpawnRegion == GizmoType.SelectedOnly)
            {
                DrawGizmos();
            }
        }

        private void DrawGizmos()
        {
            Gizmos.color = new Color(color.r, color.b, 0.3f);
            Gizmos.DrawSphere(transform.position, spawnRadius);
        }
    }
}
