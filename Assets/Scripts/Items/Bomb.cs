using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Items
{
    public class Bomb : MonoBehaviour
    {
        public void Trigger(Color color, float radius, float timeToDetonate, Tilemap map, Action<Color, Vector3Int> callback)
        {
            Debug.Log("Trigger");
            StartCoroutine(tickToExplode(color, radius, timeToDetonate, map, callback));
        }

        IEnumerator tickToExplode(Color color, float radius, float _timeToDetonate, Tilemap map, Action<Color, Vector3Int> callback)
        {
            yield return new WaitForSeconds(_timeToDetonate);
            Explode(color, radius, map, callback);
        }

        private void Explode(Color color, float radius, Tilemap map, Action<Color, Vector3Int> callback)
        {
            Debug.Log("Explode");
            Debug.Log(transform.position);
            // explosion VFX TODO
            Bounds bounds = new Bounds(transform.position, new Vector3(radius,radius,radius));
            GameManager.Instance.ZoneManager.ColorAllTiles(map, bounds, color, callback);
            Destroy(gameObject);
        }
    }
}